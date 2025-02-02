"""
This script takes the tickers in json format from https://www.sec.gov/files/company_tickers.json and populates
the stock db table with the ticker symbol and company name.
"""
import json
import os
from app.models import Stock
from tradelens import app, db


tickers_file_path = os.path.join(os.getcwd(), 'resources', 'company_tickers.json')


def populate_stocks_from_sec(ticker_file):
    """
    db model columns: ticker_symbol, company_name
    json values: ticker, title
    """
    if not os.path.exists(ticker_file):
        print(f"Error: The file {ticker_file} does not exist.")
        return

    with open(ticker_file, 'r') as file:
        data = json.load(file)

    # Check if the stocks table is empty
    if Stock.query.count() == 0:
        for item in data.values():
            ticker_symbol = item['ticker']
            company_name = item['title']

            stock = Stock(ticker_symbol=ticker_symbol, company_name=company_name)
            db.session.add(stock)

        db.session.commit()
        print("Stock data populated successfully.")
    else:
        print("Stocks table already contains data. Skipping population.")


if __name__ == '__main__':
    with app.app_context():
        populate_stocks_from_sec(tickers_file_path)
