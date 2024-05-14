"""
This script takes the tickers in json format from https://www.sec.gov/files/company_tickers.json and populates
the stock db table with the ticker symbol and company name.
"""

import json
from app.models import Stocks
from tradelens import app, db


def populate_stocks_from_json(json_file):
    with open(json_file, 'r') as file:
        data = json.load(file)

    for item in data.values():
        ticker_symbol = item['ticker']
        company_name = item['title']

        stock = Stocks(ticker_symbol=ticker_symbol, company_name=company_name)
        db.session.add(stock)

    db.session.commit()


if __name__ == '__main__':
    with app.app_context():
        populate_stocks_from_json('../company_tickers.json')
