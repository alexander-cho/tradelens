import sys
from pathlib import Path

from app.models import Stocks
from tradelens import app, db

# Add the parent directory of 'app' to the system path
current_dir = Path(__file__).resolve().parent
parent_dir = current_dir.parent
sys.path.append(str(parent_dir))


class UpdateStockUniverse:
    """
    Add or remove tickers from the db stocks table
    """
    def __init__(self, db_session):
        self.db_session = db_session

    @staticmethod
    def stock_exists(ticker):
        """
        Check if the stock already exists in the db
        """
        existing_stock = Stocks.query.filter_by(ticker_symbol=ticker).first()
        return existing_stock is not None

    def add_stock(self, ticker, company):
        """
        Add a stock to the stock universe

        Args:
            ticker: stock ticker symbol
            company: company name
        """
        pass

    def remove_stock(self, ticker):
        """
        Remove a stock from the stock universe
        """
        pass


if __name__ == '__main__':
    with app.app_context():
        update1 = UpdateStockUniverse(db.session)
        update1.add_stock('DJT', 'Trump Media and Technology Group')
