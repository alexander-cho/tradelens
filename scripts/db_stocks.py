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
