import yfinance as yf

from src.app import create_app, db

from src.app.models import Stocks

from src.modules.utils.date_ranges import get_date_range_past


def is_consolidating(df, percentage_range=2):
    # get the last three weeks of trading to scan
    recent_closes = df[-30:]
    max_close = recent_closes['Close'].max()
    min_close = recent_closes['Close'].min()

    threshold = 1 - (percentage_range / 100)

    if min_close > max_close * threshold:
        print(f"max: {max_close}, min: {min_close}, stock {stock.ticker_symbol} is consolidating")


with create_app().app_context():
    stocks = db.session.query(Stocks).filter(Stocks.id < 101)
    (start_date, today) = get_date_range_past(365)
    for stock in stocks:
        ohlcv_df = yf.download(tickers=stock.ticker_symbol, start=start_date, end=today)
        is_consolidating(ohlcv_df)
