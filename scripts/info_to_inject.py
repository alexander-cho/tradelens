import yfinance as yf
# from app.models import Stocks
# from tradelens import app, db
import warnings

warnings.filterwarnings("ignore", category=FutureWarning)


# https://awesomepython.org/?c=finance python libraries for finance
# 

# API KEY AlphaVantage
# DMZT7G4N6OHVB8VH


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

# print(ticker.options) # tuple of expiration dates
# print(ticker.options[0])
options = (ticker.option_chain(ticker.options[0]))
# print(type(options.calls))
# print(options.calls)
print(options.puts)

