import os
from urllib.request import urlopen
import ssl

import certifi
import json
from dotenv import load_dotenv

load_dotenv()


class FMP:
    def __init__(self):
        self.context = ssl.create_default_context(cafile=certifi.where())
        self.api_key = os.getenv('FMP_API_KEY')

    def get_upcoming_dividends(self, _from: str, to: str ) -> dict:
        dividends_url = f"https://financialmodelingprep.com/api/v3/stock_dividend_calendar?from={_from}&to={to}&apikey={self.api_key}"
        response = urlopen(url=dividends_url, context=self.context)
        data = response.read().decode("utf-8")
        upcoming_dividends = json.loads(data)
        return upcoming_dividends

    def get_upcoming_splits(self, _from: str, to: str) -> dict:
        splits_url = f"https://financialmodelingprep.com/api/v3/stock_split_calendar?from={_from}&to={to}&apikey={self.api_key}"
        response = urlopen(url=splits_url, context=self.context)
        data = response.read().decode("utf-8")
        ticker_dividends = json.loads(data)
        return ticker_dividends

    def get_ticker_dividends(self, ticker: str) -> dict:
        """
        Get the historical dividend dates, payouts, etc. of a ticker.

        Returns:
            dict: keys 'symbol' and 'historical', latter being a list of dictionaries for each payout
        """
        ticker_dividends_url = f"https://financialmodelingprep.com/api/v3/historical-price-full/stock_dividend/{ticker}?apikey={self.api_key}"
        response = urlopen(url=ticker_dividends_url, context=self.context)
        data = response.read().decode("utf-8")
        ticker_dividends = json.loads(data)
        return ticker_dividends

    def get_ticker_splits(self, ticker: str) -> dict:
        """
        Get the historical stock splits of a ticker.

        Returns:
            dict: keys 'symbol' and 'historical', latter being a list of dictionaries for each split
        """
        ticker_splits_url = f"https://financialmodelingprep.com/api/v3/historical-price-full/stock_split/{ticker}?apikey={self.api_key}"
        response = urlopen(url=ticker_splits_url, context=self.context)
        data = response.read().decode("utf-8")
        ticker_splits = json.loads(data)
        return ticker_splits
