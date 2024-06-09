"""
This script takes the tickers in json format from https://www.sec.gov/files/company_tickers.json and populates
the stock db table with the ticker symbol and company name.
"""
import json
import os
from app.models import Stocks
from tradelens import current_app, db

tickers_file_path = os.path.join('../..', 'resources', 'company_tickers.json')


def populate_stocks_from_sec(json_file):
    """
    db model columns: ticker_symbol, company_name
    json values: ticker, title
    """
    if not os.path.exists(json_file):
        print(f"Error: The file {json_file} does not exist.")
        return

    with open(json_file, 'r') as file:
        data = json.load(file)

    for item in data.values():
        ticker_symbol = item['ticker']
        company_name = item['title']

        stock = Stocks(ticker_symbol=ticker_symbol, company_name=company_name)
        db.session.add(stock)

    db.session.commit()


if __name__ == '__main__':
    with current_app.app_context():
        populate_stocks_from_sec(tickers_file_path)
