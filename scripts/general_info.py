import yfinance as yf


def get_fast_info(symbol):
    ticker = yf.Ticker(symbol)
    return ticker.get_fast_info()


def get_calendar(symbol):
    ticker = yf.Ticker(symbol)
    return ticker.get_calendar()
