from _yfinance import YFinance
from _finnhub import Finnhub
from _alphavantage import AlphaVantage


y = YFinance('SOFI')
f = Finnhub()
a = AlphaVantage()

import yfinance as yf

sofi = yf.Ticker('SOFI')
fast_info = sofi.get_fast_info()
