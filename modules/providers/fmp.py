import os
from urllib.request import urlopen
import ssl

import certifi
import json
from dotenv import load_dotenv

load_dotenv()

api_key = os.getenv('FMP_API_KEY')


def _create_custom_ssl_context():
    pass


# # UPCOMING DIVIDENDS
# def dividends(url):
#     context = ssl.create_default_context(cafile=certifi.where())  # Create a custom SSL context
#     response = urlopen(url, context=context)  # Pass the custom SSL context
#     data = response.read().decode("utf-8")
#     return json.loads(data)
#
#
# dividends_url = f"https://financialmodelingprep.com/api/v3/stock_dividend_calendar?from=2024-06-20&to=2024-07-20&apikey={api_key}"
# print(dividends(dividends_url))


# # UPCOMING STOCK SPLITS
# def splits(url):
#     context = ssl.create_default_context(cafile=certifi.where())  # Create a custom SSL context
#     response = urlopen(url, context=context)  # Pass the custom SSL context
#     data = response.read().decode("utf-8")
#     return json.loads(data)
#
#
# splits_url = f"https://financialmodelingprep.com/api/v3/stock_split_calendar?from=2023-08-10&to=2023-10-10&apikey={api_key}"
# print(splits(splits_url))


# # TICKER HISTORICAL DIVIDENDS
# def ticker_dividends(url):
#     context = ssl.create_default_context(cafile=certifi.where())  # Create a custom SSL context
#     response = urlopen(url, context=context)  # Pass the custom SSL context
#     data = response.read().decode("utf-8")
#     return json.loads(data)
#
#
# ticker_dividends_url = f"https://financialmodelingprep.com/api/v3/historical-price-full/stock_dividend/AAPL?apikey={api_key}"
# print(ticker_dividends(ticker_dividends_url))
#
#
# TICKER HISTORICAL SPLITS
# def ticker_splits(url):
#     context = ssl.create_default_context(cafile=certifi.where())  # Create a custom SSL context
#     response = urlopen(url, context=context)  # Pass the custom SSL context
#     data = response.read().decode("utf-8")
#     return json.loads(data)
#
#
# ticker_splits_url = f"https://financialmodelingprep.com/api/v3/historical-price-full/stock_split/AAPL?apikey={api_key}"
# print(ticker_splits(ticker_splits_url))
