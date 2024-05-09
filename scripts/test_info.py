import yfinance as yf
import warnings
import requests

warnings.filterwarnings("ignore", category=FutureWarning)


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

# print(ticker.options[0:len(ticker.options)-1])  # tuple of expiration dates
# print(ticker.option_chain().calls['contractSymbol'])
# print(ticker.financials)  # quarterly financials
# print(ticker.get_calendar())

# expiry_dates = ticker.options
# options_data = {}
# for expiry in expiry_dates:
#     options_data[expiry] = ticker.option_chain(date=expiry)
# for expiry_date, option_chain in options_data.items():
#     for i, j in option_chain.calls.iterrows():
#         print(f'Open interest: {j["openInterest"]}')
#
#
# print(type(ticker.option_chain(date='2024-05-03').calls))


# print(ticker.get_fast_info())

print(ticker.get_upgrades_downgrades())
# print(ticker.upgrades_downgrades)
