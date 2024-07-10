from flask import render_template, redirect, url_for, flash
import yfinance as yf

from app import db

from ..models import Stocks
from ..main.forms import SearchForm

from . import bp_screener

from .patterns import candlestick_patterns


@bp_screener.route('/technical-screener')
def technical_screener():
    stock_list = Stocks.query.all()
    return render_template(
        template_name_or_list='screener/technical_screener.html',
        title='Technical Screener',
        stock_list=stock_list
    )


@bp_screener.route('/screener-symbol-search', methods=["POST"])
def screener_symbol_search():
    symbol_search_form = SearchForm()
    if symbol_search_form.validate_on_submit():
        search_content = symbol_search_form.searched.data.upper()
        searched_ticker_found = Stocks.query.filter_by(ticker_symbol=search_content).first()
        if searched_ticker_found:
            return redirect(url_for('screener.symbol_screen', symbol=search_content))
        else:
            flash("That stock does not exist or is not in our database yet")
            return redirect(url_for('screener.screener_symbol_search'))
    else:
        return redirect(url_for('screener.screener_symbol_search'))


@bp_screener.route('/technical-screener/<symbol>')
def symbol_screen(symbol):
    stock = db.session.query(Stocks).filter(Stocks.ticker_symbol == symbol).first()
    patterns = candlestick_patterns
    return render_template(
        template_name_or_list='screener/symbol_screen.html',
        title=f'Screen {stock.ticker_symbol}',
        stock=stock,
        patterns=patterns
    )
