import os
from dotenv import load_dotenv
import requests
import warnings

warnings.filterwarnings(action="ignore", category=FutureWarning)

load_dotenv()


class AlphaVantage:
    def __init__(self):
        self.api_key = os.getenv('ALPHAVANTAGE_API_KEY')
        self.TOP_GAINERS_LOSERS_URL = f'https://www.alphavantage.co/query?function=TOP_GAINERS_LOSERS&apikey={self.api_key}'

    def get_top_gainers_losers(self) -> dict:
        """
        Get the 20 top gainers and losers, as well as the most actively traded stocks for the day

        Returns:
            dict: with keys ['metadata', 'last_updated', 'top_gainers', 'top_losers', 'most_actively_traded']

                'top_gainers', 'top_losers', 'most_actively_trade':
                    list of dictionaries for each ticker containing the following keys
                    ['ticker', 'price', 'change_amount', 'change_percentage', 'volume']
        """
        # request = requests.get(self.TOP_GAINERS_LOSERS_URL)
        # top_gainers_losers = request.json()
        # return top_gainers_losers

        return {
            'metadata': 'Top gainers, losers, and most actively traded US tickers',
            'last_updated': '2024-06-28 16:15:59 US/Eastern',
            'top_gainers': [
                {'ticker': 'AEAEW', 'price': '0.0599', 'change_amount': '0.0367', 'change_percentage': '158.1897%', 'volume': '1'},
                {'ticker': 'IGTAW', 'price': '0.1577', 'change_amount': '0.0843', 'change_percentage': '114.8501%', 'volume': '600'},
                {'ticker': 'IVCPW', 'price': '0.08', 'change_amount': '0.0399', 'change_percentage': '99.5012%', 'volume': '1600'},
                {'ticker': 'BITE+', 'price': '0.0625', 'change_amount': '0.0303', 'change_percentage': '94.0994%', 'volume': '19649'},
                {'ticker': 'SHFSW', 'price': '0.0699', 'change_amount': '0.0329', 'change_percentage': '88.9189%', 'volume': '11223'},
                {'ticker': 'RSVRW', 'price': '1.5', 'change_amount': '0.7', 'change_percentage': '87.5%', 'volume': '82739'},
                {'ticker': 'KVACW', 'price': '0.0749', 'change_amount': '0.0349', 'change_percentage': '87.25%', 'volume': '405'},
                {'ticker': 'LGL+', 'price': '0.285', 'change_amount': '0.13', 'change_percentage': '83.871%', 'volume': '3468'},
                {'ticker': 'TCBPW', 'price': '0.0134', 'change_amount': '0.0059', 'change_percentage': '78.6667%', 'volume': '2521'},
                {'ticker': 'KACLW', 'price': '0.009', 'change_amount': '0.0038', 'change_percentage': '73.0769%', 'volume': '18000'},
                {'ticker': 'IVCAW', 'price': '0.0433', 'change_amount': '0.0181', 'change_percentage': '71.8254%', 'volume': '13762'},
                {'ticker': 'ECDAW', 'price': '0.0284', 'change_amount': '0.0118', 'change_percentage': '71.0843%', 'volume': '2'},
                {'ticker': 'BROGW', 'price': '0.0048', 'change_amount': '0.0019', 'change_percentage': '65.5172%', 'volume': '100'},
                {'ticker': 'NCPLW', 'price': '0.0199', 'change_amount': '0.0078', 'change_percentage': '64.4628%', 'volume': '30314'},
                {'ticker': 'QOMOW', 'price': '0.0249', 'change_amount': '0.0094', 'change_percentage': '60.6452%', 'volume': '100'},
                {'ticker': 'WINVW', 'price': '0.0136', 'change_amount': '0.0051', 'change_percentage': '60.0%', 'volume': '62919'},
                {'ticker': 'SMXWW', 'price': '0.01', 'change_amount': '0.0037', 'change_percentage': '58.7302%', 'volume': '101'},
                {'ticker': 'FREEW', 'price': '0.058', 'change_amount': '0.021', 'change_percentage': '56.7568%', 'volume': '380'},
                {'ticker': 'ASNS', 'price': '2.31', 'change_amount': '0.83', 'change_percentage': '56.0811%', 'volume': '70843675'},
                {'ticker': 'CCTSW', 'price': '0.054', 'change_amount': '0.0188', 'change_percentage': '53.4091%', 'volume': '1424'}
            ],
            'top_losers': [
                {'ticker': 'AOGOW', 'price': '0.03', 'change_amount': '-0.03', 'change_percentage': '-50.0%', 'volume': '171974'},
                {'ticker': 'NUKKW', 'price': '0.023', 'change_amount': '-0.0217', 'change_percentage': '-48.5459%', 'volume': '25596'},
                {'ticker': 'AGRIW', 'price': '0.0042', 'change_amount': '-0.0039', 'change_percentage': '-48.1481%', 'volume': '47992'},
                {'ticker': 'PTIXW', 'price': '0.0082', 'change_amount': '-0.0075', 'change_percentage': '-47.7707%', 'volume': '11009'},
                {'ticker': 'ABLVW', 'price': '0.0257', 'change_amount': '-0.0232', 'change_percentage': '-47.4438%', 'volume': '66'},
                {'ticker': 'BTOG', 'price': '2.88', 'change_amount': '-2.44', 'change_percentage': '-45.8647%', 'volume': '3364244'},
                {'ticker': 'CING', 'price': '0.32', 'change_amount': '-0.264', 'change_percentage': '-45.2055%', 'volume': '2445874'},
                {'ticker': 'ACCD', 'price': '3.58', 'change_amount': '-2.81', 'change_percentage': '-43.975%', 'volume': '17643230'},
                {'ticker': 'NXL', 'price': '1.69', 'change_amount': '-1.27', 'change_percentage': '-42.9054%', 'volume': '4454117'},
                {'ticker': 'IVCBW', 'price': '0.0468', 'change_amount': '-0.0344', 'change_percentage': '-42.3645%', 'volume': '1489'},
                {'ticker': 'APVO', 'price': '0.306', 'change_amount': '-0.2089', 'change_percentage': '-40.571%', 'volume': '2456552'},
                {'ticker': 'ELTX', 'price': '4.11', 'change_amount': '-2.78', 'change_percentage': '-40.3483%', 'volume': '405659'},
                {'ticker': 'NUVOW', 'price': '0.0213', 'change_amount': '-0.0136', 'change_percentage': '-38.9685%', 'volume': '221'},
                {'ticker': 'TRONW', 'price': '0.03', 'change_amount': '-0.0188', 'change_percentage': '-38.5246%', 'volume': '1401'},
                {'ticker': 'RCKTW', 'price': '0.0603', 'change_amount': '-0.0365', 'change_percentage': '-37.7066%', 'volume': '69485'},
                {'ticker': 'RMCOW', 'price': '0.0176', 'change_amount': '-0.0104', 'change_percentage': '-37.1429%', 'volume': '1999'},
                {'ticker': 'SRZNW', 'price': '0.0166', 'change_amount': '-0.0094', 'change_percentage': '-36.1538%', 'volume': '935'},
                {'ticker': 'SAVA', 'price': '12.35', 'change_amount': '-6.6', 'change_percentage': '-34.8285%', 'volume': '17119668'},
                {'ticker': 'BGXX', 'price': '0.2533', 'change_amount': '-0.1318', 'change_percentage': '-34.2249%', 'volume': '7546480'},
                {'ticker': 'LFLYW', 'price': '0.0155', 'change_amount': '-0.0074', 'change_percentage': '-32.3144%', 'volume': '5653'}
            ],
            'most_actively_traded': [
                {'ticker': 'LUCY', 'price': '0.495', 'change_amount': '0.165', 'change_percentage': '50.0%', 'volume': '360231526'},
                {'ticker': 'NVDA', 'price': '123.49', 'change_amount': '-0.5', 'change_percentage': '-0.4033%', 'volume': '307349222'},
                {'ticker': 'DNA', 'price': '0.331', 'change_amount': '0.0218', 'change_percentage': '7.0505%', 'volume': '224462989'},
                {'ticker': 'FFIE', 'price': '0.5066', 'change_amount': '-0.0934', 'change_percentage': '-15.5667%', 'volume': '193194462'},
                {'ticker': 'SQQQ', 'price': '8.28', 'change_amount': '0.15', 'change_percentage': '1.845%', 'volume': '154537445'},
                {'ticker': 'NKE', 'price': '75.36', 'change_amount': '-18.83', 'change_percentage': '-19.9915%', 'volume': '128860802'},
                {'ticker': 'NYCB', 'price': '3.21', 'change_amount': '0.17', 'change_percentage': '5.5921%', 'volume': '127293778'},
                {'ticker': 'RIG', 'price': '5.345', 'change_amount': '0.015', 'change_percentage': '0.2814%', 'volume': '127216940'},
                {'ticker': 'NU', 'price': '12.9', 'change_amount': '-0.02', 'change_percentage': '-0.1548%', 'volume': '120746907'},
                {'ticker': 'PLUG', 'price': '2.325', 'change_amount': '-0.135', 'change_percentage': '-5.4878%', 'volume': '113063586'},
                {'ticker': 'TSLA', 'price': '197.88', 'change_amount': '0.46', 'change_percentage': '0.233%', 'volume': '94691983'},
                {'ticker': 'OPTT', 'price': '0.1965', 'change_amount': '0.0365', 'change_percentage': '22.8125%', 'volume': '89241753'},
                {'ticker': 'TELL', 'price': '0.7176', 'change_amount': '-0.041', 'change_percentage': '-5.4047%', 'volume': '87991010'},
                {'ticker': 'RIVN', 'price': '13.42', 'change_amount': '-1.05', 'change_percentage': '-7.2564%', 'volume': '86746567'},
                {'ticker': 'COSM', 'price': '1.04', 'change_amount': '0.36', 'change_percentage': '52.9412%', 'volume': '80606544'},
                {'ticker': 'SIRI', 'price': '2.83', 'change_amount': '0.1', 'change_percentage': '3.663%', 'volume': '80501480'},
                {'ticker': 'AUR', 'price': '2.77', 'change_amount': '0.04', 'change_percentage': '1.4652%', 'volume': '79920387'},
                {'ticker': 'PR', 'price': '16.13', 'change_amount': '0.16', 'change_percentage': '1.0019%', 'volume': '79708400'},
                {'ticker': 'AAPL', 'price': '210.76', 'change_amount': '-3.34', 'change_percentage': '-1.56%', 'volume': '77839794'},
                {'ticker': 'AMZN', 'price': '193.25', 'change_amount': '-4.6', 'change_percentage': '-2.325%', 'volume': '76637648'}
            ]
        }
