import os
from dotenv import load_dotenv

from fredapi import Fred

load_dotenv()


class FederalReserve:
    """
    Federal Reserve API class for fetching macroeconomic data
    """
    def __init__(self):
        self.api_key = os.getenv('FRED_API_KEY')
        self.fred = Fred(api_key=self.api_key)

    def get_gdp(self):
        return self.fred.get_series('GDP')


federal_reserve = FederalReserve()
print(federal_reserve.get_gdp())
