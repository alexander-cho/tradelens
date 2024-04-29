# import sys
# from pathlib import Path
#
# # Add the parent directory of 'app' to the system path
# current_dir = Path(__file__).resolve().parent
# parent_dir = current_dir.parent
# sys.path.append(str(parent_dir))
#
#
# from app.models import Stocks
# import yfinance as yf
# from tradelens import app, db
import warnings
#
#
warnings.filterwarnings("ignore", category=FutureWarning)
#
#
# # clear data first
# def clear_ohlcv():
#     stocks = Stocks.query.all()
#     for stock in stocks:
#         stock.open = None
#         stock.high = None
#         stock.low = None
#         stock.close = None
#         stock.volume = None
#     db.session.commit()
#
#
# def populate_ohlcv():
#     # query all the ticker symbols from stocks table
#     stocks = Stocks.query.all()
#     for stock in stocks:
#         ticker = yf.Ticker(stock.ticker_symbol)
#         hist_data = ticker.history(period="1d")
#
#         # Get the first row which corresponds to the last trading day
#         last_day = hist_data.iloc[0]
#
#         stock.open = last_day['Open']
#         stock.high = last_day['High']
#         stock.low = last_day['Low']
#         stock.close = last_day['Close']
#         stock.volume = last_day['Volume']
#
#         stock.last_price = ticker.get_fast_info()['lastPrice']
#
#         try:
#             stock.shares_outstanding = ticker.info['sharesOutstanding']
#         except Exception as e:
#             print(f"Error fetching implied shares for {stock.ticker_symbol}: {e}")
#             stock.shares_outstanding = None
#
#     db.session.commit()
#
#     # add method that records time it takes for populate_ohlcv method to execute
#
#
# if __name__ == '__main__':
#     with app.app_context():
#         clear_ohlcv()
#         populate_ohlcv()


# RENAME TO get_fast_info.py

import yfinance as yf


tick = yf.Ticker('SOFI')
print(tick.history(period='1d'))


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
