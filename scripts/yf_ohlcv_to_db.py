import sys
from pathlib import Path

# Add the parent directory of 'app' to the system path
current_dir = Path(__file__).resolve().parent
parent_dir = current_dir.parent
sys.path.append(str(parent_dir))


from app.models import Stocks
import yfinance as yf
from tradelens import app, db
import warnings


warnings.filterwarnings("ignore", category=FutureWarning)

#clear data first
def clear_ohlcv():
    stocks = Stocks.query.all()
    for stock in stocks:
        stock.open = None
        stock.high = None
        stock.low = None
        stock.close = None
        stock.volume = None
    db.session.commit()


def populate_ohlcv():
    # query all of the ticker symbols from stocks table
    stocks = Stocks.query.all()
    for stock in stocks:
        ticker = yf.Ticker(stock.ticker_symbol)
        hist_data = ticker.history(period="1d")

        # Get the first row which corresponds to the last trading day
        last_day = hist_data.iloc[0]

        stock.open = last_day['Open']
        stock.high = last_day['High']
        stock.low = last_day['Low']
        stock.close = last_day['Close']
        stock.volume = last_day['Volume']

        stock.last_price = ticker.get_fast_info()['lastPrice']
        try:
            stock.shares_outstanding = ticker.info['impliedSharesOutstanding']
        except:
            stock.shares_outstanding = None

    db.session.commit()

def populate_others():
    stocks = Stocks.query.all()
    for stock in stocks:
        # print(f"Processing {stock.ticker_symbol}")
        ticker = yf.Ticker(stock.ticker_symbol)
        try:
            stock.shares_outstanding = ticker.info['impliedSharesOutstanding']
        except Exception as e:
            print(f"Error fetching implied shares for {stock.ticker_symbol}: {e}")
            stock.shares_outstanding = None

    db.session.commit()


if __name__ == '__main__':
    with app.app_context():
        # clear_ohlcv()
        # populate_ohlcv()
        populate_others()