from flask import render_template

import sqlalchemy as sa

from app import db

from ..models import Stocks

from src.providers.yfinance_ import YFinance
from src.providers.tradier import Tradier

from . import bp_options


@bp_options.route('/options/<symbol>')
def options(symbol):
    stock = db.session.scalar(sa.select(Stocks).where(Stocks.ticker_symbol == symbol))

    yfinance = YFinance(symbol)
    expiry_list = yfinance.get_expiry_list()

    return render_template('options/options.html',
                           title=f"{symbol} - Options Expiration Calendar",
                           stock=stock,
                           expiry_list=expiry_list)


@bp_options.route('/options/<symbol>/<expiry_date>')
def options_expiry(symbol, expiry_date):
    stock = db.session.scalar(sa.select(Stocks).where(Stocks.ticker_symbol == symbol))

    yfinance = YFinance(symbol)
    tradier = Tradier()

    open_interest = yfinance.get_open_interest(expiry_date)
    volume = yfinance.get_volume(expiry_date)
    implied_volatility = yfinance.get_implied_volatility(expiry_date)
    last_bid_ask = yfinance.get_last_price_bid_ask(expiry_date)

    tradier_options = tradier.get_options_chain(symbol, expiry_date)

    return render_template('options/options_expiry.html',
                           title=f'{symbol} {expiry_date}',
                           stock=stock,
                           expiry_date=expiry_date,
                           open_interest=open_interest,
                           volume=volume,
                           implied_volatility=implied_volatility,
                           last_bid_ask=last_bid_ask,
                           tradier_options=tradier_options)
