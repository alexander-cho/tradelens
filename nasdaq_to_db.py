from models import Stocks
from tradelens import app, db
import csv


def nasdaq_to_db(filename):
    with open(filename, 'r', encoding='utf-8') as file:
        reader = csv.reader(file) # create a csv reader object
        next(reader) # skip the header row
        for row in reader:
            ticker_symbol, company_name = row[:2] # get the first two cols: ticker symbol and company name
            stock = Stocks(ticker_symbol=ticker_symbol, company_name=company_name) # create a Stocks object with the extracted data
            db.session.add(stock)
        db.session.commit()


if __name__ == '__main__':
    with app.app_context():
        nasdaq_to_db('nasdaq.csv')