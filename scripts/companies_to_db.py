'''
This script takes the nasdaq.csv file downloaded from the nasdaq website and populates a Stocks table with the data from
the first two columns
'''
import os
import sys
from pathlib import Path

# Add the parent directory of 'app' to the system path
current_dir = Path(__file__).resolve().parent
parent_dir = current_dir.parent
sys.path.append(str(parent_dir))

from app.models import Stocks
from app import app, db
import csv


def companies_to_db(filename):
    with open(filename, 'r', encoding='utf-8') as file:
        reader = csv.reader(file) # create a csv reader object
        next(reader) # skip the header row
        for row in reader:
            ticker_symbol, company_name = row[1], row[3] # get the columns: ticker symbol and company name
            stock = Stocks(ticker_symbol=ticker_symbol, company_name=company_name) # create a Stocks object with the extracted data
            db.session.add(stock)
        db.session.commit()


if __name__ == '__main__':
    with app.app_context():
        companies_to_db('companies.csv')