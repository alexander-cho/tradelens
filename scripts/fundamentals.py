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


# clear data first
def clear_fundamentals():
    stocks = Stocks.query.all()
    for stock in stocks:
        stock.earnings_forecasts = None

    db.session.commit()


def populate_fundamentals():
    # query all the ticker symbols from stocks table
    stocks = Stocks.query.all()
    for stock in stocks:
        ticker = yf.Ticker(stock.ticker_symbol)
        
        stock.earnings_forecasts = ticker.get_calendar()
        
    db.session.commit()

# ticker.get_calendar() is a dict type, I need to either index it or inject it as a whole into a column


if __name__ == '__main__':
    with app.app_context():
        populate_fundamentals()
