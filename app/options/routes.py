from flask import render_template, redirect, url_for, flash

import sqlalchemy as sa

from app import db

from ..models import Stocks

from modules.providers.yfinance_ import YFinance
from modules.providers.tradier import Tradier

from . import bp_options


@bp_options.route('/options/<symbol>')
def options(symbol):
    stock = db.session.scalar(sa.select(Stocks).where(Stocks.ticker_symbol == symbol))

    yfinance = YFinance(symbol)
    expiry_list = yfinance.get_expiry_list()

    return render_template('options/options_calendar.html',
                           title=f"{symbol} - Options Expiration Calendar",
                           stock=stock,
                           expiry_list=expiry_list)


@bp_options.route('/options/<symbol>/<expiry_date>')
def options_chain(symbol, expiry_date):
    yfinance = YFinance(symbol)
    expiry_list = yfinance.get_expiry_list()

    # check if user enters or looks for invalid expiration date by checking if it is in the list of dates
    if expiry_date not in expiry_list:
        flash("That expiry date does not exist")
        return redirect(url_for('options.options', symbol=symbol))

    stock = db.session.scalar(sa.select(Stocks).where(Stocks.ticker_symbol == symbol))

    tradier = Tradier(stock.ticker_symbol)

    open_interest = tradier._get_open_interest(expiry_date)
    volume = tradier._get_volume(expiry_date)
    implied_volatility = tradier._get_implied_volatility(expiry_date)
    last_bid_ask = tradier._get_last_bid_ask(expiry_date)

    return render_template('options/options_chain.html',
                           title=f'{symbol} {expiry_date}',
                           stock=stock,
                           expiry_date=expiry_date,
                           open_interest=open_interest,
                           volume=volume,
                           implied_volatility=implied_volatility,
                           last_bid_ask=last_bid_ask)


@bp_options.route('/options/<symbol>/<expiry_date>/greeks')
def greeks(symbol, expiry_date):
    stock = db.session.scalar(sa.select(Stocks).where(Stocks.ticker_symbol == symbol))

    tradier = Tradier(stock.ticker_symbol)
    tradier_greeks = tradier.get_options_chain(expiry_date)
    return render_template('options/greeks.html',
                           title=f"Greeks for {symbol} {expiry_date}",
                           stock=stock,
                           expiry_date=expiry_date,
                           tradier_greeks=tradier_greeks)
