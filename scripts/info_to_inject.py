import yfinance as yf
# from app.models import Stocks
# from tradelens import app, db
import warnings

warnings.filterwarnings("ignore", category=FutureWarning)


# https://awesomepython.org/?c=finance python libraries for finance
# 



btc = yf.Ticker('BTC.X')
# print(sofi.major_holders)
# print(sofi.balance_sheet)

'''
print(sofi.get_calendar())

<class 'dict'> of length 7
{'Earnings Date': [datetime.date(2024, 4, 29), datetime.date(2024, 5, 3)], 
'Earnings High': 0.04, 
'Earnings Low': -0.02, 
'Earnings Average': 0.01,
'Revenue High': 662000000, 
'Revenue Low': 541900000, 
'Revenue Average': 568380000}
'''


# print(sofi.get_fast_info()['lastPrice']) # this gets live volume??

print(btc.info) # outstanding shares

