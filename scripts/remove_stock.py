import sys
from pathlib import Path

# Add the parent directory of 'app' to the system path
current_dir = Path(__file__).resolve().parent
parent_dir = current_dir.parent
sys.path.append(str(parent_dir))


from app.models import Stocks
from tradelens import app, db
# from add_stock import stock_exists


# check if the stock already exists in the table
def stock_exists(ticker):
    existing_stock = Stocks.query.filter_by(ticker_symbol=ticker).first()
    return existing_stock is not None


def remove_stock(ticker):
    if stock_exists(ticker):
        Stocks.query.filter(Stocks.ticker_symbol == ticker).delete()
        db.session.commit()
        print(f"'{ticker}' has been deleted.")
    else:
        print(f"Cannot delete '{ticker}', which does not exist in table.")


if __name__ == '__main__':
    with app.app_context():
        remove_stock('DWAC')
