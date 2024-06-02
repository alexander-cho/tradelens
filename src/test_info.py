from _yfinance import YFinance
from _finnhub import Finnhub
from _alphavantage import AlphaVantage
import yfinance as yf


y = YFinance('SOFI')
f = Finnhub()
a = AlphaVantage()

sofi = yf.Ticker('SOFI')

ohlcv = sofi.history(period='1d')
ohlcv_dict = ohlcv.to_dict(orient='records')
for k in ohlcv_dict:
    print(k)
