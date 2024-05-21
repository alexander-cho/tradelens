import csv
import requests
import warnings


warnings.filterwarnings("ignore", category=FutureWarning)


class AlphaVantage:
    IPO_URL = 'https://www.alphavantage.co/query?function=IPO_CALENDAR&apikey=GLLVZKDV4221RMO6'
    EARNINGS_URL = 'https://www.alphavantage.co/query?function=EARNINGS_CALENDAR&apikey=GLLVZKDV4221RMO6'
    BALANCE_SHEET_URL = 'https://www.alphavantage.co/query?function=BALANCE_SHEET&symbol=SOFI&apikey=GLLVZKDV4221RMO6'

    def __init__(self):
        self.ipo_list = []
        self.earnings_list = []

    def get_ipos_data(self):
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
                low_listing_price = ipo[3]
                high_listing_price = ipo[4]

                # some listings are ETFs, or other securities without a listing price, so we'll filter them out
                if low_listing_price != '0' and high_listing_price != '0':
                    filtered_ipo_list.append(ipo)

        return filtered_ipo_list

    def get_earnings_calendar(self):
        pass

    def get_balance_sheet(self):
        r = requests.get(self.BALANCE_SHEET_URL)
        data = r.json()
        return data


av = AlphaVantage()
print(av.get_ipos_data())
