import sys
from pathlib import Path

from app.models import Stocks
from tradelens import current_app, db

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
    def _stock_exists(ticker: str) -> bool:
        """
        Check if the stock already exists in the db

        Parameters:
            ticker (str): stock ticker

        Returns:
            bool: True if the stock exists, False otherwise
        """
        existing_stock = Stocks.query.filter_by(ticker_symbol=ticker).first()
        return existing_stock is not None

    def add_stock(self, ticker: str, company: str):
        """
        Add a stock to the stock universe

        Parameters:
            ticker (str): stock ticker symbol
            company (str): company name
        """
        if not self._stock_exists(ticker):
            new_stock = Stocks(ticker_symbol=ticker, company_name=company)
            db.session.add(new_stock)
            db.session.commit()
            print(f"'{ticker}' added.")
        else:
            print(f"Stock {ticker} already exists")

        return

    def remove_stock(self, ticker: str):
        """
        Remove a stock from the stock universe

        Parameters:
            ticker (str): stock ticker symbol
        """
        if self._stock_exists(ticker):
            Stocks.query.filter(Stocks.ticker_symbol == ticker).delete()
            db.session.commit()
            print(f"'{ticker}' has been deleted.")
        else:
            print(f"Cannot delete '{ticker}', which does not exist in table.")

        return


if __name__ == '__main__':
    with current_app.app_context():
        update1 = UpdateStockUniverse(db.session)
