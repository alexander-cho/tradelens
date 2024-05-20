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

            if len(self.ipo_list) == 1:
                print("No IPOs available at the moment")
            else:
                return self.ipo_list

    def get_earnings_calendar(self):
        pass

    def get_balance_sheet(self):
        r = requests.get(self.BALANCE_SHEET_URL)
        data = r.json()
        return data
