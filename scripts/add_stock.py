import sys
from pathlib import Path

# Add the parent directory of 'app' to the system path
current_dir = Path(__file__).resolve().parent
parent_dir = current_dir.parent
sys.path.append(str(parent_dir))

from app.models import Stocks
from tradelens import app, db


# check if the stock already exists in the table
def stock_exists(ticker):
    existing_stock = Stocks.query.filter_by(ticker_symbol=ticker).first()
    return existing_stock is not None


def add_stock(ticker, company):
    if not stock_exists(ticker):
        new_stock = Stocks(ticker_symbol=ticker, company_name=company)
        db.session.add(new_stock)
        db.session.commit()
        print(f"'{ticker}' added.")
    else:
        print(f"'{ticker}' already exists.")


if __name__ == '__main__':
    with app.app_context():
        add_stock('DJT', 'Trump Media & Technology Group Corp.')

