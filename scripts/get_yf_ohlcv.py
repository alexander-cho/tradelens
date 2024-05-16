import warnings

import yfinance as yf


warnings.filterwarnings("ignore", category=FutureWarning)


def get_ohlcv(symbol):
    ticker = yf.Ticker(symbol)
    return ticker.history(period='1d')


def get_shares_outstanding(symbol):
    ticker = yf.Ticker(symbol)
    try:
        outstanding_shares = ticker.info['sharesOutstanding']
        return outstanding_shares
    except Exception as e:
        print(f"Error fetching implied shares for {symbol}: {e}")


def get_underlying_for_daily_change(symbol):
    ticker = yf.Ticker(symbol)
    underlying = ticker.option_chain().underlying
    return underlying
