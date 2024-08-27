import sys
from pathlib import Path

from ...app.models import Stock
from tradelens import app, db

# Add the parent directory of 'app' to the system path
current_dir = Path(__file__).resolve().parent
parent_dir = current_dir.parent
sys.path.append(str(parent_dir))


class UpdateStockUniverse:
    """
    Add or remove tickers from the db stocks table
    """
    def __init__(self):
        self.db_session = db.session

    @staticmethod
    def _stock_exists(ticker: str) -> bool:
        """
        Check if the stock already exists in the db

        Parameters:
            ticker (str): stock ticker

        Returns:
            bool: True if the stock exists, False otherwise
        """
        existing_stock = Stock.query.filter_by(ticker_symbol=ticker).first()
        return existing_stock is not None

    def add_stock(self, ticker: str, company: str) -> None:
        """
        Add a stock to the stock universe

        Parameters:
            ticker (str): stock ticker symbol
            company (str): company name
        """
        if not self._stock_exists(ticker):
            new_stock = Stock(ticker_symbol=ticker, company_name=company)
            db.session.add(new_stock)
            db.session.commit()
            print(f"'{ticker}' added.")
        else:
            print(f"Stock {ticker} already exists")

        return

    def remove_stock(self, ticker: str) -> None:
        """
        Remove a stock from the stock universe

        Parameters:
            ticker (str): stock ticker symbol
        """
        if self._stock_exists(ticker):
            Stock.query.filter(Stock.ticker_symbol == ticker).delete()
            db.session.commit()
            print(f"'{ticker}' has been deleted.")
        else:
            print(f"Cannot delete '{ticker}', which does not exist in table.")

        return


if __name__ == '__main__':
    with app.app_context():
        update1 = UpdateStockUniverse()
