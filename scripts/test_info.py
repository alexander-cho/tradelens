import yfinance as yf
import warnings
import requests
import finnhub
import csv


warnings.filterwarnings("ignore", category=FutureWarning)


ticker = yf.Ticker('SOFI')
# print(ticker.info)



# replace the "demo" apikey below with your own key from https://www.alphavantage.co/support/#api-key
CSV_URL = 'https://www.alphavantage.co/query?function=EARNINGS_CALENDAR&horizon=1week&apikey=GLLVZKDV4221RMO6'

with requests.Session() as s:
    download = s.get(CSV_URL)
    decoded_content = download.content.decode('utf-8')
    cr = csv.reader(decoded_content.splitlines(), delimiter=',')
    my_list = list(cr)
    for row in my_list:
        print(row)