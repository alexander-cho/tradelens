import yfinance as yf
import warnings
import requests
import finnhub

import csv


warnings.filterwarnings("ignore", category=FutureWarning)


ticker = yf.Ticker('SOFI')
option_chain = ticker.option_chain(date='2024-05-24').calls

# Create the list of dictionaries
strike_volume_list = [{row['strike']: row['volume']} for index, row in option_chain.iterrows()]

# Print the result
print(strike_volume_list)
