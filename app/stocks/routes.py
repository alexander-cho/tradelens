from flask import render_template, flash, redirect, url_for, request

from app import db

from ..models import Stock, Post

from ..main.forms import SearchForm
from ..feed.forms import PostForm

from ..feed.routes import add_post

from modules.providers.yfinance_ import YFinance
from modules.providers.finnhub_ import Finnhub
from modules.providers.polygon_ import Polygon
from modules.providers.fmp import FMP

from modules.utils.date_ranges import get_date_range_past

from . import bp_stocks


# symbol directory route
@bp_stocks.route('/symbol')
def symbol_directory():
    stock_list = Stock.query.all()
    return render_template(
        template_name_or_list='stocks/symbol_directory.html',
        title='Symbol Directory',
        stock_list=stock_list
    )


# search for a company
@bp_stocks.route('/symbol-search', methods=['POST'])
def symbol_search():
    symbol_search_form = SearchForm()
    if symbol_search_form.validate_on_submit():
        search_content = symbol_search_form.searched.data.upper()
        searched_ticker_found = Stock.query.filter_by(ticker_symbol=search_content).first()
        if searched_ticker_found:
            return redirect(url_for('stocks.symbol', symbol=search_content))
        else:
            flash("That stock does not exist or is not in our database yet")
            return redirect(url_for('stocks.symbol_directory'))
    else:
        return redirect(url_for('stocks.symbol_directory'))


# display information for each company in the stocks table
@bp_stocks.route('/symbol/<symbol>', methods=['GET', 'POST'])
def symbol(symbol):
    # query the stock table to retrieve the corresponding symbol
    stock = db.session.query(Stock).filter(Stock.ticker_symbol == symbol).first()

    # if user manually enters the ticker in lowercase letters in the url
    symbol = symbol.upper()
    if request.path != f"/symbol/{symbol}":
        return redirect(url_for('stocks.symbol', symbol=symbol))

    # if the stock does not exist
    if not stock:
        flash("That stock does not exist or is not in our database yet")
        return redirect(url_for('stocks.symbol_directory'))

    (chart_past_date, chart_today) = get_date_range_past(days_past=365)
    polygon = Polygon(
        ticker=f'{symbol}',
        multiplier=1,
        timespan='day',
        from_=f'{chart_past_date}',
        to=f'{chart_today}',
        limit=50000
    )
    stock_chart = polygon.make_chart()

    # CONTEXT FROM MODULES FOR SYMBOL DATA
    yfinance = YFinance(stock.ticker_symbol)
    finnhub = Finnhub()

    company_profile = finnhub.get_company_profile(ticker=stock.ticker_symbol)
    main_info = yfinance.get_underlying_for_price_info()
    basic_info = yfinance.get_basic_info()
    fast_info = yfinance.get_fast_info()
    calendar = yfinance.get_calendar()

    # # ADDING A POST ON THE SYMBOL PAGE
    post_form = PostForm()
    if post_form.validate_on_submit():
        add_post()

    # query the posts associated with the symbol
    symbol_posts = Post.query.order_by(Post.timestamp.desc()).where(Post.title == symbol).all()

    return render_template(
        template_name_or_list='stocks/symbol.html',
        title=f'{stock.company_name} ({stock.ticker_symbol})',
        stock=stock,
        stock_chart=stock_chart,
        company_profile=company_profile,
        main_info=main_info,
        basic_info=basic_info,
        fast_info=fast_info,
        calendar=calendar,
        post_form=post_form,
        symbol_posts=symbol_posts
    )


@bp_stocks.route('/symbol/<symbol>/news')
def symbol_news(symbol):
    stock = db.session.query(Stock).filter(Stock.ticker_symbol == symbol).first()

    # if user manually enters the ticker in lowercase letters in the url
    symbol = symbol.upper()
    if request.path != f"/symbol/{symbol}/news":
        return redirect(url_for('stocks.symbol_news', symbol=symbol))

    finnhub = Finnhub()

    (past_date, today) = get_date_range_past(days_past=7)
    ticker_news = finnhub.get_stock_news(ticker=stock.ticker_symbol, _from=past_date, to=today)

    return render_template(
        template_name_or_list='stocks/symbol_news.html',
        title=f'News for {stock.ticker_symbol}',
        stock=stock,
        ticker_news=ticker_news
    )


@bp_stocks.route('/symbol/<symbol>/financials')
def symbol_financials(symbol):
    stock = db.session.query(Stock).filter(Stock.ticker_symbol == symbol).first()

    # if user manually enters the ticker in lowercase letters in the url
    symbol = symbol.upper()
    if request.path != f"/symbol/{symbol}/financials":
        return redirect(url_for('stocks.symbol_financials', symbol=symbol))

    yfinance = YFinance(stock.ticker_symbol)

    balance_sheet = yfinance.plot_balance_sheet()
    cashflow_statement = yfinance.plot_cashflow_statement()
    income_statement = yfinance.plot_income_statement()

    return render_template(
        template_name_or_list='stocks/symbol_financials.html',
        title=f'{stock.ticker_symbol} Financials',
        stock=stock,
        balance_sheet=balance_sheet,
        cashflow_statement=cashflow_statement,
        income_statement=income_statement
    )


@bp_stocks.route('/symbol/<symbol>/holders')
def symbol_holders(symbol):
    stock = db.session.query(Stock).filter(Stock.ticker_symbol == symbol).first()

    # if user manually enters the ticker in lowercase letters in the url
    symbol = symbol.upper()
    if request.path != f"/symbol/{symbol}/holders":
        return redirect(url_for('stocks.symbol_holders', symbol=symbol))

    yfinance = YFinance(stock.ticker_symbol)
    finnhub = Finnhub()

    institutional_holders = yfinance.plot_institutional_holders()
    analyst_ratings = yfinance.get_analyst_ratings()

    (sentiment_past_date, sentiment_today) = get_date_range_past(days_past=365)
    (transactions_past_date, transactions_today) = get_date_range_past(days_past=182)
    insider_sentiment = finnhub.plot_insider_sentiment(ticker=stock.ticker_symbol, _from=sentiment_past_date, to=sentiment_today)
    insider_transactions = finnhub.plot_insider_transactions(ticker=stock.ticker_symbol, _from=transactions_past_date, to=transactions_today)

    return render_template(
        template_name_or_list='stocks/symbol_holders.html',
        title=f'{stock.company_name} ({stock.ticker_symbol}) - Holders',
        stock=stock,
        institutional_holders=institutional_holders,
        insider_transactions=insider_transactions,
        analyst_ratings=analyst_ratings,
        insider_sentiment=insider_sentiment
    )


@bp_stocks.route('/symbol/<symbol>/federal')
def symbol_federal(symbol):
    stock = db.session.query(Stock).filter(Stock.ticker_symbol == symbol).first()

    # if user manually enters the ticker in lowercase letters in the url
    symbol = symbol.upper()
    if request.path != f"/symbol/{symbol}/federal":
        return redirect(url_for('stocks.symbol_federal', symbol=symbol))

    finnhub = Finnhub()

    (past_date, today) = get_date_range_past(days_past=365)
    lobbying_activities = finnhub.get_lobbying_activities(ticker=symbol, _from=past_date, to=today)
    government_spending = finnhub.get_government_spending(ticker=symbol, _from=past_date, to=today)

    return render_template(
        template_name_or_list='stocks/symbol_federal.html',
        title=f'{stock.company_name} ({stock.ticker_symbol}) - Federal',
        stock=stock,
        lobbying_activities=lobbying_activities,
        government_spending=government_spending
    )


@bp_stocks.route('/symbol/<symbol>/dividends-splits')
def symbol_dividends_splits(symbol):
    stock = db.session.query(Stock).filter(Stock.ticker_symbol == symbol).first()

    # if user manually enters the ticker in lowercase letters in the url
    symbol = symbol.upper()
    if request.path != f"/symbol/{symbol}/dividends-splits":
        return redirect(url_for('stocks.symbol_dividends_splits', symbol=symbol))

    fmp = FMP()

    ticker_dividends = fmp.get_ticker_dividends(ticker=stock.ticker_symbol)
    ticker_splits = fmp.get_ticker_splits(ticker=stock.ticker_symbol)

    return render_template(
        template_name_or_list='stocks/symbol_dividends_splits.html',
        title=f'{stock.ticker_symbol} Dividends and Splits',
        stock=stock,
        ticker_dividends=ticker_dividends,
        ticker_splits=ticker_splits
    )
