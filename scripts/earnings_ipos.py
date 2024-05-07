import csv
import requests
import warnings


warnings.filterwarnings("ignore", category=FutureWarning)


IPO_URL = 'https://www.alphavantage.co/query?function=IPO_CALENDAR&apikey=GLLVZKDV4221RMO6'
EARNINGS_URL = 'https://www.alphavantage.co/query?function=EARNINGS_CALENDAR&apikey=GLLVZKDV4221RMO6'


def get_ipos_data(csv_url):
    with requests.Session() as s:
        download = s.get(csv_url)
        decoded_content = download.content.decode('utf-8')
        cr = csv.reader(decoded_content.splitlines(), delimiter=',')
        my_list = list(cr)
        return my_list


def get_earnings_calendar():
    pass
