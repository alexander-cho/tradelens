import os
import sys
from pathlib import Path

# Add the parent directory of 'app' to the system path
current_dir = Path(__file__).resolve().parent
parent_dir = current_dir.parent
sys.path.append(str(parent_dir))


from app.models import Stocks
from tradelens import app, db
import warnings


warnings.filterwarnings("ignore", category=FutureWarning)


import csv
import requests


CSV_URL = 'https://www.alphavantage.co/query?function=IPO_CALENDAR&apikey=GLLVZKDV4221RMO6'

with requests.Session() as s:
    download = s.get(CSV_URL)
    decoded_content = download.content.decode('utf-8')
    cr = csv.reader(decoded_content.splitlines(), delimiter=',')
    my_list = list(cr)
    for row in my_list:
        print(row)