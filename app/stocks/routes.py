from flask import render_template, flash, redirect, url_for, request

from app import db

from ..models import Stocks, Post

from ..main.forms import SearchForm
from ..feed.forms import PostForm

from ..feed.routes import add_post

from modules.providers.yfinance_ import YFinance
from modules.providers.finnhub_ import Finnhub

from modules.utils.date_ranges import get_date_range_past

from . import bp_stocks


# symbol directory route
@bp_stocks.route('/symbol')
def symbol_main():
    stock_list = Stocks.query.all()
    return render_template('stocks/symbol_main.html',
                           title='Symbol Directory',
                           stock_list=stock_list)


# search for a company
@bp_stocks.route('/symbol-search', methods=['POST'])
def symbol_search():
    symbol_search_form = SearchForm()
    if symbol_search_form.validate_on_submit():
        search_content = symbol_search_form.searched.data.upper()
        searched_ticker_found = Stocks.query.filter_by(ticker_symbol=search_content).first()
        if searched_ticker_found:
            return redirect(url_for('stocks.symbol',
                                    symbol=search_content))
        else:
            flash("That stock does not exist or is not in our database yet")
            return redirect(url_for('stocks.symbol_main'))
    else:
        return redirect(url_for('stocks.symbol_main'))


# display information for each company in the stocks table
@bp_stocks.route('/symbol/<symbol>', methods=['GET', 'POST'])
def symbol(symbol):
    # if user manually enters the ticker in lowercase letters in the url
    symbol = symbol.upper()
    if request.path != f"/symbol/{symbol}":
        return redirect(url_for('stocks.symbol',
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

    # # ADDING A POST ON THE SYMBOL PAGE
    post_form = PostForm()
    if post_form.validate_on_submit():
        add_post()

    return render_template('stocks/symbol.html',
                           title=f'{stock.company_name} ({stock.ticker_symbol})',
                           stock=stock,
                           post_form=post_form,
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


@bp_stocks.route('/symbol/<symbol>/news')
def symbol_news(symbol):
    stock = db.session.query(Stocks).filter(Stocks.ticker_symbol == symbol).first()

    finnhub = Finnhub()

    (past_date, today) = get_date_range_past(days_past=7)
    ticker_news = finnhub.get_stock_news(ticker=stock.ticker_symbol, _from=past_date, to=today)

    return render_template('stocks/symbol_news.html',
                           title=f'News for {symbol}',
                           stock=stock,
                           ticker_news=ticker_news)
