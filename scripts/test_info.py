import yfinance as yf
import warnings
import requests
import finnhub

warnings.filterwarnings("ignore", category=FutureWarning)


ticker = yf.Ticker('SOFI')
