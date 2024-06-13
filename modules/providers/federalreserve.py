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
        monthly_interest_rates = self.fred.get_series('FEDFUNDS').to_dict()
        return monthly_interest_rates

    def get_unemployment_rate(self) -> dict:
        monthly_unemployment_rate = self.fred.get_series('UNRATE').to_dict()
        return monthly_unemployment_rate

    def get_10yr(self) -> dict:
        ten_year_yield = self.fred.get_series('DGS10').to_dict()
        return ten_year_yield

    def get_trade_balance(self) -> dict:
        """
        Get the monthly trade balance (trade deficit) data from 1992 to present

        Returns:
            dict: Monthly trade deficit data of key value pairs of type Timestamp and float
        """
        trade_balance = self.fred.get_series('BOPGSTB').to_dict()
        return trade_balance

    def get_capacity_utilization(self) -> dict:
        """
        Get the capacity utilization as a percentage of the maximum potential output of the economy can produce
        under normal operating conditions without straining its resources
        """
        capacity_utilization = self.fred.get_series('TCU').to_dict()
        return capacity_utilization

    def get_payroll(self) -> dict:
        total_nonfarm_payroll = self.fred.get_series('PAYEMS').to_dict()
        return total_nonfarm_payroll

    def get_housing_data(self) -> dict:
        units_started = self.fred.get_series('HOUST').to_dict()
        median_sales_price = self.fred.get_series('MSPUS').to_dict()

        housing_data = {
            'units_started': units_started,
            'median_sales_price': median_sales_price
        }

        return housing_data

    def get_commodities(self) -> dict:
        oil_prices = self.fred.get_series('MCOILWTICO').to_dict()
        natural_gas_prices = self.fred.get_series('MHHNGSP').to_dict()
        sugar_prices = self.fred.get_series('PSUGAUSAUSDM').to_dict()
        corn_prices = self.fred.get_series('PMAIZMTUSDM').to_dict()

        commodities = {
            "oil_prices": oil_prices,
            "natural_gas_prices": natural_gas_prices,
            "sugar_prices": sugar_prices,
            "corn_prices": corn_prices
        }

        return commodities
