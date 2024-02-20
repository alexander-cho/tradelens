from tradelens import app, db
from models import Stocks
import json
import matplotlib.pyplot as plt

'''
The institutional ownership data is populated in the stocks table as a valid JSON data type. We convert it to a list type
for ease of operations for further analysis
'''

with app.app_context():
    sofi = Stocks.query.filter_by(ticker_symbol='SOFI').first()
    sofi_list = json.loads(sofi.institutional_info)
    for i in sofi_list:
        shareholder, share_count = i['Holder'], i['Shares']
        print(shareholder, share_count)

    plt.figure(figsize=(10, 6))
    plt.bar(shareholder, share_count, color='skyblue')
    plt.xlabel('Holder')
    plt.ylabel('Number of Shares')
    plt.title('Number of Shares Held by Institutional Holders')
    plt.xticks(rotation=45, ha='right')
    plt.tight_layout()
    plt.show()

    #FAULTY PLOT