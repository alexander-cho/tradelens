import yfinance as yf
# from app.models import Stocks
# from tradelens import app, db
import warnings
import requests

warnings.filterwarnings("ignore", category=FutureWarning)


# https://awesomepython.org/?c=finance python libraries for finance

# API KEY AlphaVantage
# GLLVZKDV4221RMO6


ticker = yf.Ticker('SOFI')
# print(sofi.major_holders)
# print(sofi.balance_sheet)


# print(ticker.get_calendar()['Earnings Date']) # get the estimated next earnings date

# <class 'dict'> of length 7
# {'Earnings Date': [datetime.date(2024, 4, 29), datetime.date(2024, 5, 3)], 
# 'Earnings High': 0.04, 
# 'Earnings Low': -0.02, 
# 'Earnings Average': 0.01,
# 'Revenue High': 662000000, 
# 'Revenue Low': 541900000, 
# 'Revenue Average': 568380000}


# print(ticker.get_fast_info()['lastPrice']) # this gets live volume??

# try:
#     print(ticker.info['impliedSharesOutstanding']) # outstanding shares
# except:
#     print('not available')

# print(ticker.info['sharesOutstanding'])
# print(type(ticker.get_calendar()['Earnings Average']))

# print(ticker.options)  # tuple of expiration dates
# print(ticker.option_chain().calls['contractSymbol'])
# print(ticker.financials)  # quarterly financials
# print(ticker.get_calendar())

url = 'https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol=IBM&interval=5min&apikey=GLLVZKDV4221RMO6'
r = requests.get(url)
data = r.json()

print(data)
