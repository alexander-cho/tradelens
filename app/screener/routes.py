import talib
import yfinance as yf

from flask import render_template, request

from app import db

from ..models import Stocks

from modules.utils.date_ranges import get_date_range_past

from . import bp_screener

from app.screener.patterns import candlestick_patterns


@bp_screener.route('/technical-screener')
def technical_screener():
    patterns = candlestick_patterns
    stocks = db.session.query(Stocks).filter(Stocks.id < 101)
    (start_date, today) = get_date_range_past(365)

    # form select in template, name attribute is pattern, get the current selected pattern using this
    current_pattern = request.args.get('pattern', None)
    # pattern names are named attributes in ta-lib implementation, so pass the name of the function which is a string
    # and pass is as a variable
    pattern_data = {}
    if current_pattern:
        for stock in stocks:
            pattern_function = getattr(talib, current_pattern)
            try:
                ohlcv_df = yf.download(tickers=stock.ticker_symbol, start=start_date, end=today)
                result = pattern_function(ohlcv_df['Open'], ohlcv_df['High'], ohlcv_df['Low'], ohlcv_df['Close'])
                # return just the last candlestick
                last = result.tail(1).values[0]
                if last > 0:
                    pattern_data[stock.ticker_symbol] = 'bullish'
                elif last < 0:
                    pattern_data[stock.ticker_symbol] = 'bearish'
                else:
                    pass
            except ValueError:
                pass

    screen = {
        'pattern': current_pattern,
        'data': pattern_data
    }

    return render_template(
        template_name_or_list='screener/technical_screener.html',
        title='Technical Screener',
        stocks=stocks,
        patterns=patterns,
        current_pattern=current_pattern,
        screen=screen
    )
