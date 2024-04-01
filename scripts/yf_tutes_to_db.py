import os
import sys
from pathlib import Path

# Add the parent directory of 'app' to the system path
current_dir = Path(__file__).resolve().parent
parent_dir = current_dir.parent
sys.path.append(str(parent_dir))


import yfinance as yf
from app.models import Stocks
from tradelens import app, db
import warnings


warnings.filterwarnings("ignore", category=FutureWarning)

# clear data first
def clear_institutional_info():
    stocks = Stocks.query.all()
    for stock in stocks:
        stock.institutional_info = None
    db.session.commit()


def populate_institutional_info():
    # query all of the ticker symbols from stocks table
    stocks = Stocks.query.all()
    for stock in stocks:
        try:
            ticker = yf.Ticker(stock.ticker_symbol)
            tute_df = ticker.get_institutional_holders()
            df_to_json = tute_df.to_json(orient='records')
            stock.institutional_info = df_to_json

        except Exception as e:
            print(f"Error fetching data for {stock.ticker_symbol}: {e}")

    db.session.commit()


if __name__ == '__main__':
    with app.app_context():
        clear_institutional_info()
        populate_institutional_info()
