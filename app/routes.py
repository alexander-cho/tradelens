from datetime import datetime, timezone

from flask import render_template, flash, redirect, url_for, request
from flask_login import login_user, current_user, logout_user, login_required

import sqlalchemy as sa

from app import app, db

from .models import Stocks, Post

from .feed.forms import SearchForm

from src.providers.yfinance_ import YFinance
from src.providers.alphavantage import AlphaVantage
from src.providers.finnhub_ import Finnhub
from src.providers.federalreserve import FederalReserve
from src.providers.tradier import Tradier

from src.utils.date_ranges import get_date_range_ahead, get_date_range_past


@app.before_request
def before_request():
    # if current_user is logged in
    if current_user.is_authenticated:
        # set last seen field to current time
        current_user.last_seen = datetime.now(timezone.utc)
        db.session.commit()


# Pass search form to navbar
@app.context_processor
def base():
    form = SearchForm()
    return dict(form=form)


# front page
@app.route('/')
@app.route('/index')
# @login_required
def index():
    finnhub = Finnhub()
    alphavantage = AlphaVantage()

    market_status = finnhub.get_market_status(),
    top_gainers_losers = alphavantage.get_top_gainers_losers()

    return render_template('index.html',
                           title='Home',
                           market_status=market_status,
                           top_gainers_losers=top_gainers_losers)


# symbol directory route
@app.route('/symbol')
def symbol_main(): 
    stock_list = Stocks.query.all() 
    return render_template('symbol_main.html',
                           title='Symbol Directory',
                           stock_list=stock_list)


# search for a company
@app.route('/symbol-search', methods=['POST'])
def symbol_search():
    form = SearchForm()
    if form.validate_on_submit():
        search_content = form.searched.data.upper()
        searched_ticker_found = Stocks.query.filter_by(ticker_symbol=search_content).first()
        if searched_ticker_found:
            return redirect(url_for('symbol',
                                    symbol=search_content))
        else:
            flash("That stock does not exist or is not in our database yet")
            return redirect(url_for('symbol_main'))
    else:
        return redirect(url_for('symbol_main'))


# display information for each company in the stocks table
@app.route('/symbol/<symbol>', methods=['GET', 'POST'])
def symbol(symbol):
    # if user manually enters the ticker in lowercase letters in the url
    symbol = symbol.upper()
    if request.path != f"/symbol/{symbol}":
        return redirect(url_for('symbol',
                                symbol=symbol))

    # query the stock table to retrieve the corresponding symbol
    stock = db.session.query(Stocks).filter(Stocks.ticker_symbol == symbol).first()
    # query the posts associated with the symbol
    symbol_posts = Post.query.order_by(Post.timestamp.desc()).where(Post.title == symbol)

    # CONTEXT FROM SRC FOR SYMBOL DATA
    yfinance = YFinance(symbol)
    finnhub = Finnhub()

    ohlcv_data = yfinance.get_day_ohlcv()
    basic_info = yfinance.get_info()
    main_info = yfinance.get_underlying_for_price_info()
    fast_info = yfinance.get_fast_info()
    calendar = yfinance.get_calendar()
    institutional_holders = yfinance.get_institutional_holders()
    insider_transactions = yfinance.get_insider_transactions()
    analyst_ratings = yfinance.get_analyst_ratings()
    company_profile = finnhub.get_company_profile(ticker=symbol)

    (past_date, today) = get_date_range_past(days_past=365)
    insider_sentiment = finnhub.get_insider_sentiment(ticker=symbol, _from=past_date, to=today)
    lobbying_activities = finnhub.get_lobbying_activities(ticker=symbol, _from=past_date, to=today)
    government_spending = finnhub.get_government_spending(ticker=symbol, _from=past_date, to=today)

    # # # ADDING A POST ON THE SYMBOL PAGE
    # form = PostForm()
    # if form.validate_on_submit():
    #     add_post()

    return render_template('symbol.html',
                           title=f'{stock.company_name} ({stock.ticker_symbol})',
                           stock=stock,
                           # form=form,
                           symbol_posts=symbol_posts,
                           ohlcv_data=ohlcv_data,
                           basic_info=basic_info,
                           main_info=main_info,
                           fast_info=fast_info,
                           calendar=calendar,
                           institutional_holders=institutional_holders,
                           insider_transactions=insider_transactions,
                           analyst_ratings=analyst_ratings,
                           company_profile=company_profile,
                           insider_sentiment=insider_sentiment,
                           lobbying_activities=lobbying_activities,
                           government_spending=government_spending)


# return IPOs anticipated in the next 3 months, upcoming earnings calendar
@app.route('/earnings-ipos', methods=['GET'])
def earnings_ipos():
    alphavantage = AlphaVantage()
    finnhub = Finnhub()

    ipo_data = alphavantage.get_ipos_data()

    (today, future_date) = get_date_range_ahead(days_ahead=14)
    earnings_calendar = finnhub.get_earnings_calendar(_from=today, to=future_date)

    return render_template('earnings_ipos.html',
                           title='IPOs',
                           ipo_data=ipo_data,
                           earnings_calendar=earnings_calendar)


@app.route('/options/<symbol>')
def options(symbol):
    stock = db.session.scalar(sa.select(Stocks).where(Stocks.ticker_symbol == symbol))

    yfinance = YFinance(symbol)
    expiry_list = yfinance.get_expiry_list()

    return render_template('options.html',
                           title=f"{symbol} - Options Expiration Calendar",
                           stock=stock,
                           expiry_list=expiry_list)


@app.route('/options/<symbol>/<expiry_date>')
def options_expiry(symbol, expiry_date):
    stock = db.session.scalar(sa.select(Stocks).where(Stocks.ticker_symbol == symbol))

    yfinance = YFinance(symbol)
    tradier = Tradier()

    open_interest = yfinance.get_open_interest(expiry_date)
    volume = yfinance.get_volume(expiry_date)
    implied_volatility = yfinance.get_implied_volatility(expiry_date)
    last_bid_ask = yfinance.get_last_price_bid_ask(expiry_date)

    tradier_options = tradier.get_options_chain(symbol, expiry_date)

    return render_template('options_expiry.html',
                           title=f'{symbol} {expiry_date}',
                           stock=stock,
                           expiry_date=expiry_date,
                           open_interest=open_interest,
                           volume=volume,
                           implied_volatility=implied_volatility,
                           last_bid_ask=last_bid_ask,
                           tradier_options=tradier_options)


@app.route('/symbol/<symbol>/news')
def symbol_news(symbol):
    stock = db.session.query(Stocks).filter(Stocks.ticker_symbol == symbol).first()

    finnhub = Finnhub()

    (past_date, today) = get_date_range_past(days_past=7)
    ticker_news = finnhub.get_stock_news(ticker=stock.ticker_symbol, _from=past_date, to=today)

    return render_template('symbol_news.html',
                           title=f'News for {symbol}',
                           stock=stock,
                           ticker_news=ticker_news)


@app.route('/macro')
def macro():
    federal_reserve = FederalReserve()

    gdp = federal_reserve.get_quarterly_gdp()
    cpi = federal_reserve.get_cpi()

    return render_template('macro.html',
                           title='Macro',
                           gdp=gdp,
                           cpi=cpi)


@app.route('/technical-screener')
def technical_screener():
    return render_template('technical_screener.html',
                           title='Technical Screener')


@app.route('/market-news/<category>', methods=['GET'])
def market_news(category):
    category = category.lower()
    # market news categories
    valid_categories = ['general', 'forex', 'crypto', 'merger']
    if category not in valid_categories:
        flash(f"Invalid news category: {category}. Showing general news instead.")
        category = 'general'
        return redirect(url_for('market_news', category=category))

    finnhub = Finnhub()
    try:
        news = finnhub.get_market_news(category)
    except ValueError as e:
        flash(str(e))
        return redirect(url_for('market_news', category='general'))

    return render_template('market_news.html',
                           title=f"{category.capitalize()} News",
                           category=category,
                           news=news)


# if __name__ == '__main__':
#     app.run(debug=True)
