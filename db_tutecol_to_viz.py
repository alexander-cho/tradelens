from tradelens import app, db
from models import Stocks
import json

'''
The institutional ownership data is populated in the stocks table as a valid JSON data type. We convert it to a list type
for ease of operations for further analysis
'''

with app.app_context():
    sofi = Stocks.query.filter_by(ticker_symbol='SOFI').first()
    sofi_list = json.loads(sofi.institutional_info)
    for i in sofi_list:
        print(i)