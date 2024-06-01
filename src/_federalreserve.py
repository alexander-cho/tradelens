import os
from dotenv import load_dotenv
import pandas as pd

from fredapi import Fred

load_dotenv()


class FederalReserve:
    """
    Federal Reserve API class for fetching macroeconomic data
    """
    def __init__(self):
        self.api_key = os.getenv('FRED_API_KEY')
        self.fred = Fred(api_key=self.api_key)

    def get_quarterly_gdp(self) -> dict:
        """
        Get the quarterly gdp data from 1947 to present

        Returns:
            dict: Quarterly GDP data of key value pairs of type Timestamp and float
        """
        quarterly_gdp = self.fred.get_series('GDP').to_dict()
        filtered_quarterly_gdp = {}
        # remove nan values
        for k, v in quarterly_gdp.items():
            if pd.notna(v):
                filtered_quarterly_gdp[k] = v

        return filtered_quarterly_gdp

    def get_cpi(self) -> dict:
        """
        Get the monthly YoY consumer price changes from 1956 to present

        Returns:
            dict: Monthly CPI of key value pairs of type Timestamp and float
        """
        monthly_cpi = self.fred.get_series('CPALTT01USM659N').to_dict()
        return monthly_cpi

    def get_interest_rates(self):
        pass

    def unemployment_rates(self) -> dict:
        pass

    def get_ppi(self) -> dict:
        pass

    def get_cci(self) -> dict:
        pass


federal_reserve = FederalReserve()
print(federal_reserve.get_cpi())
