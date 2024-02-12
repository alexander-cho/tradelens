from models import Stocks
import yfinance as yf
from tradelens import app, db
import warnings

warnings.filterwarnings("ignore", category=FutureWarning)


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

    db.session.commit()


if __name__ == '__main__':
    with app.app_context():
        populate_ohlcv()