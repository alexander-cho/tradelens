import yfinance as yf
import pandas as pd
from models import Stocks
from tradelens import app, db
import warnings

warnings.filterwarnings("ignore", category=FutureWarning)


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
        populate_institutional_info()
