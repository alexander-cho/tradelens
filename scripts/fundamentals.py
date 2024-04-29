import yfinance as yf
import requests

# BALANCE SHEET
url = 'https://www.alphavantage.co/query?function=BALANCE_SHEET&symbol=SOFI&apikey=GLLVZKDV4221RMO6'
r = requests.get(url)
data = r.json()

print(data)


# YF EARNINGS FORECAST
ticker = yf.Ticker('SOFI')
print(ticker.get_calendar())
