import csv
import os
from dotenv import load_dotenv
import requests
import warnings

warnings.filterwarnings("ignore", category=FutureWarning)

load_dotenv()


class AlphaVantage:
    def __init__(self):
        self.api_key = os.getenv('ALPHA_VANTAGE_API_KEY')
        self.IPO_URL = f'https://www.alphavantage.co/query?function=IPO_CALENDAR&apikey={self.api_key}'
        self.EARNINGS_URL = f'https://www.alphavantage.co/query?function=EARNINGS_CALENDAR&apikey={self.api_key}'
        self.BALANCE_SHEET_URL = f'https://www.alphavantage.co/query?function=BALANCE_SHEET&symbol=SOFI&apikey={self.api_key}'
        self.TOP_GAINERS_LOSERS_URL = f'https://www.alphavantage.co/query?function=TOP_GAINERS_LOSERS&apikey={self.api_key}'
        self.ipo_list = []
        self.earnings_list = []

    def get_ipos_data(self) -> list:
        """
        Fetch and parse the IPO list for upcoming market IPOs

        Returns:
            list: List of IPO data rows, first list is the headers
        """
        with requests.Session() as s:
            download = s.get(self.IPO_URL)
            decoded_content = download.content.decode('utf-8')
            cr = csv.reader(decoded_content.splitlines(), delimiter=',')
            self.ipo_list = list(cr)

            filtered_ipo_list = []

            for ipo in self.ipo_list:
                low_listing_price = ipo[3]  # index of low listing price
                high_listing_price = ipo[4]  # index of high listing price

                # filter ETFs and other securities without a listing price
                if low_listing_price != '0' and high_listing_price != '0':
                    filtered_ipo_list.append(ipo)

        return filtered_ipo_list

    def get_earnings_calendar(self):
        """

        """
        pass

    def get_balance_sheet(self):
        """

        """
        r = requests.get(self.BALANCE_SHEET_URL)
        data = r.json()
        return data

    def get_top_gainers_losers(self) -> dict:
        """
        Get the 20 top gainers and losers, as well as the most actively traded stocks for the day

        Returns:
            dict: with keys ['metadata', 'last_updated', 'top_gainers', 'top_losers', 'most_actively_traded']

                'top_gainers', 'top_losers', 'most_actively_trade':
                    list of dictionaries for each ticker containing the following keys
                    ['ticker', 'price', 'change_amount', 'change_percentage', 'volume']
        """
        r = requests.get(self.TOP_GAINERS_LOSERS_URL)
        data = r.json()
        return data
