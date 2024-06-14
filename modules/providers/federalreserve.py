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
        Get the quarterly gdp data

        Returns:
            dict: Quarterly GDP data of key value pairs of type Timestamp and float, in billions
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
        Get the monthly YoY consumer price changes

        Returns:
            dict: Monthly CPI of key value pairs of type Timestamp and float, which represents percentage change
        """
        monthly_cpi = self.fred.get_series('CPALTT01USM659N').to_dict()
        return monthly_cpi

    def get_interest_rates(self):
        """
        Get the monthly interest rate (Fed Funds) reading

        Returns:
            dict: Monthly interest rates of key value pairs of type Timestamp and float, expressed as a whole
            percentage
        """
        monthly_interest_rates = self.fred.get_series('FEDFUNDS').to_dict()
        return monthly_interest_rates

    def get_unemployment_rate(self) -> dict:
        """
        Get the monthly unemployment rate

        Returns:
            dict: Monthly unemployment rate of key value pairs of type Timestamp and float, expressed as a percentage
        """
        monthly_unemployment_rate = self.fred.get_series('UNRATE').to_dict()
        return monthly_unemployment_rate

    def get_10yr(self) -> dict:
        """
        Get the daily change for the 10-year treasury yield

        Returns:
            dict: Daily changes of key value pairs of type Timestamp and float, expressed as a percentage
        """
        ten_year_yield = self.fred.get_series('DGS10').to_dict()
        return ten_year_yield

    def get_trade_balance(self) -> dict:
        """
        Get the monthly trade balance (trade deficit) data

        Returns:
            dict: Monthly trade deficit data of key value pairs of type Timestamp and float,
            expressed in millions of dollars
        """
        trade_balance = self.fred.get_series('BOPGSTB').to_dict()
        return trade_balance

    def get_capacity_utilization(self) -> dict:
        """
        Get the capacity utilization as a percentage of the maximum potential output of the economy can produce
        under normal operating conditions without straining its resources

        Returns:
            dict: Return monthly capacity utilization of key value pairs of type Timestamp and float, represented as
            a percentage
        """
        capacity_utilization = self.fred.get_series('TCU').to_dict()
        return capacity_utilization

    def get_payroll(self) -> dict:
        """
        Get the monthly series of nonfarm payroll employees

        Returns:
            dict: Monthly series data of nonfarm payroll of key value pairs of type Timestamp and float,
            expressed in thousands of employees
        """
        total_nonfarm_payroll = self.fred.get_series('PAYEMS').to_dict()
        return total_nonfarm_payroll

    def get_housing_data(self) -> dict:
        """
        Get the following:
        - Newly started construction of privately-owned housing units each month,
            as key value pairs of type Timestamp and float, expressed in thousands of units
        - Quarterly median sales price for a home

        Returns:
            dict: Above information as key value pairs
        """
        units_started = self.fred.get_series('HOUST').to_dict()
        median_sales_price = self.fred.get_series('MSPUS').to_dict()

        housing_data = {
            'units_started': units_started,
            'median_sales_price': median_sales_price
        }

        return housing_data

    def get_commodities(self) -> dict:
        """
        Get the following:
        - Monthly recorded oil prices, as key value pairs of type Timestamp and float, expressed in dollars per barrel
        - Monthly recorded natural gas prices, as key value pairs of type Timestamp and float,
            expressed in dollars per million BTU
        - Monthly recorded global price of sugar, as key value pairs of type Timestamp and float,
            expressed in US cents per pound
        - Monthly recorded global corn prices, as key value pairs of type Timestamp and float,
            expressed in US dollars per metric ton

        Returns:
            dict: Above information as key value pairs
        """
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
