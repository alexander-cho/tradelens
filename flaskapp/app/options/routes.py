from flask import render_template, redirect, url_for, flash, request

import sqlalchemy as sa

from app import db

from ..models import Stock

from modules.providers.yfinance_ import YFinance
from modules.providers.tradier import Tradier
from modules.providers.polygon_ import Polygon

from modules.utils.date_ranges import get_date_range_past

from modules.max_pain import MaxPain

from . import bp_options


@bp_options.route('/options/<symbol>')
def options(symbol):
    stock = db.session.scalar(sa.select(Stock).where(Stock.ticker_symbol == symbol))

    # if user manually enters the ticker in lowercase letters in the url
    symbol = symbol.upper()
    if request.path != f"/options/{symbol}":
        return redirect(url_for('options.options', symbol=symbol))

    # if the stock does not exist
    if not stock:
        flash("That stock does not exist or is not in our database yet. No option chain available.")
        return redirect(url_for('stocks.symbol_directory'))

    yfinance = YFinance(symbol)
    expiry_list = yfinance.get_options_expiry_list()

    return render_template(
        template_name_or_list='options/options_calendar.html',
        title=f"{symbol} - Options Expiration Calendar",
        stock=stock,
        expiry_list=expiry_list
    )


@bp_options.route('/options/<symbol>/<expiry_date>')
def options_chain(symbol, expiry_date):
    yfinance = YFinance(symbol)
    expiry_list = yfinance.get_options_expiry_list()

    # check if user enters or looks for invalid expiration date by checking if it is in the list of dates
    if expiry_date not in expiry_list.get('data'):
        flash("That expiry date does not exist")
        return redirect(url_for('options.options', symbol=symbol))

    stock = db.session.scalar(sa.select(Stock).where(Stock.ticker_symbol == symbol))

    current_price = yfinance.get_underlying_for_price_info().get('data').get('regularMarketPrice')

    tradier = Tradier(stock.ticker_symbol)

    open_interest = tradier.plot_open_interest(expiration_date=expiry_date)
    volume = tradier.plot_volume(expiration_date=expiry_date)
    implied_volatility = tradier.plot_iv(expiration_date=expiry_date)
    last_bid_ask = tradier.plot_last_bid_ask(expiration_date=expiry_date)

    return render_template(
        template_name_or_list='options/options_chain.html',
        title=f'{symbol} {expiry_date}',
        stock=stock,
        expiry_date=expiry_date,
        current_price=current_price,
        open_interest=open_interest,
        volume=volume,
        implied_volatility=implied_volatility,
        last_bid_ask=last_bid_ask
    )


@bp_options.route('/options/<symbol>/<expiry_date>/maximum-pain')
def maximum_pain(symbol, expiry_date):
    stock = db.session.scalar(sa.select(Stock).where(Stock.ticker_symbol == symbol))

    yfinance = YFinance(symbol=stock.ticker_symbol)
    current_price = yfinance.get_underlying_for_price_info().get('data').get('regularMarketPrice')

    max_pain = MaxPain(stock.ticker_symbol)
    cash_values_chart = max_pain.plot_cash_values(expiration_date=expiry_date)
    cash_values = max_pain.get_cash_values(expiration_date=expiry_date)

    return render_template(
        template_name_or_list='options/maximum_pain.html',
        title=f"Maximum Pain - {symbol} {expiry_date}",
        stock=stock,
        current_price=current_price,
        cash_values_chart=cash_values_chart,
        cash_values=cash_values,
        expiry_date=expiry_date
    )


@bp_options.route('/options/<symbol>/<expiry_date>/strikes')
def options_strikes(symbol, expiry_date):
    stock = db.session.scalar(sa.select(Stock).where(Stock.ticker_symbol == symbol))

    yfinance = YFinance(symbol)
    current_price = yfinance.get_underlying_for_price_info().get('data').get('regularMarketPrice')

    tradier = Tradier(stock.ticker_symbol)

    strikes = tradier.get_strikes(expiration_date=expiry_date)

    return render_template(
        template_name_or_list='options/strikes.html',
        title=f"Strikes for {symbol} expiring on {expiry_date}",
        stock=stock,
        current_price=current_price,
        expiry_date=expiry_date,
        strikes=strikes
    )


@bp_options.route('/options/<symbol>/<expiry_date>/strikes/<option_ticker>')
def options_chart(symbol, expiry_date, option_ticker):
    stock = db.session.scalar(sa.select(Stock).where(Stock.ticker_symbol == symbol))

    yfinance = YFinance(symbol=stock.ticker_symbol)
    current_price = yfinance.get_underlying_for_price_info().get('data').get('regularMarketPrice')

    (chart_past_date, chart_today) = get_date_range_past(days_past=14)
    polygon = Polygon(
        ticker=f'O:{option_ticker}',
        multiplier=1,
        timespan='hour',
        from_=f'{chart_past_date}',
        to=f'{chart_today}',
        limit=50000
    )

    option_chart = polygon.make_chart()

    return render_template(
        template_name_or_list='options/option_chart.html',
        title=f"{option_ticker}",
        stock=stock,
        current_price=current_price,
        expiry_date=expiry_date,
        option_ticker=option_ticker,
        option_chart=option_chart
    )


@bp_options.route('/options/<symbol>/<expiry_date>/greeks')
def greeks(symbol, expiry_date):
    stock = db.session.scalar(sa.select(Stock).where(Stock.ticker_symbol == symbol))

    yfinance = YFinance(symbol=stock.ticker_symbol)
    current_price = yfinance.get_underlying_for_price_info().get('data').get('regularMarketPrice')

    tradier = Tradier(stock.ticker_symbol)
    delta = tradier.plot_greeks(expiration_date=expiry_date, greek_letter='delta')
    gamma = tradier.plot_greeks(expiration_date=expiry_date, greek_letter='gamma')
    theta = tradier.plot_greeks(expiration_date=expiry_date, greek_letter='theta')
    vega = tradier.plot_greeks(expiration_date=expiry_date, greek_letter='vega')
    rho = tradier.plot_greeks(expiration_date=expiry_date, greek_letter='rho')

    return render_template(
        template_name_or_list='options/greeks.html',
        title=f"Greeks for {symbol} {expiry_date}",
        stock=stock,
        current_price=current_price,
        expiry_date=expiry_date,
        delta=delta,
        gamma=gamma,
        theta=theta,
        vega=vega,
        rho=rho
    )
