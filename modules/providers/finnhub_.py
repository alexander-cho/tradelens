import os
from datetime import datetime
import pytz

from dotenv import load_dotenv

import finnhub

load_dotenv()


class Finnhub:
    def __init__(self):
        self.api_key = os.getenv('FINNHUB_API_KEY')
        self.fc = finnhub.Client(api_key=self.api_key)

    def get_market_status(self) -> dict:
        """
        Get the current market status for US exchanges

        Returns:
            dict: market status
        """
        response = self.fc.market_status(exchange='US')

        # Get the current time in PST
        pst_tz = pytz.timezone('America/Los_Angeles')
        current_pst_time = datetime.now(pst_tz)

        # Insert the current PST time and timezone into the market_status dictionary
        response['t'] = current_pst_time.strftime('%Y-%m-%d %H:%M:%S')
        response['timezone'] = 'America/Los_Angeles'

        return response

    def get_market_holidays(self) -> dict:
        """
        Get the current market holidays for US exchanges

        Returns:
            dict: market holidays for the current year
        """
        response = self.fc.market_holiday(exchange='US')
        holidays = response.get('data', [])

        # reverse list in place
        holidays.reverse()

        # get the current year
        current_year = str(datetime.now().year)

        # for each holiday dictionary item in the list, remove the ones not within the current year
        # among the ones that are, elaborate early or full closure
        for holiday in holidays[:]:
            if current_year not in holiday['atDate']:
                holidays.remove(holiday)
            else:
                if holiday.get('tradingHour') == '':
                    holiday['tradingHour'] = 'Market Closed'
                else:
                    holiday['tradingHour'] = holiday.get('tradingHour') + '(Early Close)'

        return {
            'description': 'Market Holidays',
            'year': current_year,
            'data': holidays
        }

    def get_market_news(self, category: str) -> list[dict]:
        """
        Get overall market news

        Returns:
            list: List of dictionaries each containing info for news article
        """
        market_news = self.fc.general_news(category=category, min_id=0)
        return market_news

    def get_stock_news(self, ticker: str, _from: str, to: str) -> list[dict]:
        """
        Get recent news for a specific stock

        Parameters:
            ticker (str): ticker symbol
            _from (str): start date (YYYY-MM-DD)
            to (str): end date (YYYY-MM-DD)

        Returns:
            list: list of dictionaries, each containing news data from an external article
            keys: ['category', 'datetime', 'headline', 'id', 'image', 'related', 'source', 'summary', 'url']
        """
        ticker_news = self.fc.company_news(symbol=ticker, _from=_from, to=to)
        return ticker_news

    def get_company_profile(self, ticker: str) -> dict:
        """
        Get the basic profile data for a company, and return certain keys of use

        Parameters:
            ticker (str): ticker symbol

        Returns:
            dict: basic profile data, which includes the web URL and company logo
        """
        company_profile = self.fc.company_profile2(symbol=ticker)
        keys = ['ticker', 'ipo', 'weburl', 'logo', 'finnhubIndustry']
        condensed_profile = {}

        for key in keys:
            if key in company_profile:
                condensed_profile[key] = company_profile[key]

        return condensed_profile

    def get_insider_sentiment(self, ticker: str, _from: str, to: str) -> dict:
        """
        Get the insider sentiment data for a specific stock, based on monthly purchases.
        Metric called MSPR (monthly share purchase ratio) is used, if market purchases > market sells, sentiment
        is considered positive.

        The closer this number is to 100 (-100) the more reliable that the stock prices of the firm
        increase (decrease) in the next periods.

        Parameters:
            ticker (str): ticker symbol
            _from (str): start date (YYYY-MM-DD)
            to (str): end date (YYYY-MM-DD)

        Returns:
            dict containing keys such as "change" which refers to the net buying/selling of insiders in number of shares
            "mspr": monthly share purchase ratio
        """
        insider_sentiment = self.fc.stock_insider_sentiment(symbol=ticker, _from=_from, to=to)
        return insider_sentiment

    def get_lobbying_activities(self, ticker: str, _from: str, to: str) -> dict:
        """
        Get the company's congressional lobbying activities within a time frame

        Parameters:
            ticker (str): ticker symbol
            _from (str): start date (YYYY-MM-DD)
            to (str): end date (YYYY-MM-DD)

        Returns:
            dict: keys "data", "symbol"
            "data": list of dictionaries with specific keys pertaining to one lobbying activity
        """
        lobbying_activities = self.fc.stock_lobbying(symbol=ticker, _from=_from, to=to)
        # initialize empty list to pack with dictionaries containing the wanted keys from full API response
        filtered_data = []
        # for each activity in the "data" list
        for activity in lobbying_activities["data"]:
            # these are the key value pairs we want to include
            filtered_activity = {
                "year": activity.get("year"),
                "period": activity.get("period"),
                "type": activity.get("type"),
                "documentUrl": activity.get("documentUrl"),
                "income": activity.get("income"),
                "expenses": activity.get("expenses")
            }
            filtered_data.append(filtered_activity)

        # return as dict
        return {"data": filtered_data,
                "symbol": lobbying_activities["symbol"]}

    def get_government_spending(self, ticker: str, _from: str, to: str) -> dict:
        """
        Get a particular company's government spending activities within a time frame.
        Identify large government contracts.

        Parameters:
            ticker (str): ticker symbol
            _from (str): start date (YYYY-MM-DD)
            to (str): end date (YYYY-MM-DD)

        Returns:
            dict: keys "data", "symbol"
            "data": list of dictionaries with specific keys pertaining to a government contract or expense.
        """
        government_spending = self.fc.stock_usa_spending(symbol=ticker, _from=_from, to=to)

        # initialize empty list to pack with dictionaries containing the wanted keys from full API response
        filtered_data = []
        # for each activity in the "data" list
        for activity in government_spending["data"]:
            # these are the key value pairs we want to include
            filtered_activity = {
                "totalValue": activity.get("totalValue"),
                "actionDate": activity.get("actionDate"),
                "awardingAgencyName": activity.get("awardingAgencyName"),
                "awardingOfficeName": activity.get("awardingOfficeName"),
                "awardDescription": activity.get("awardDescription"),
                "permalink": activity.get("permalink")
            }
            filtered_data.append(filtered_activity)

        # return as dict
        return {"data": filtered_data,
                "symbol": government_spending["symbol"]}

    def get_earnings_calendar(self, _from: str, to: str) -> dict:
        """
        Get the earnings calendar of anticipated earnings reports for a specified date range.

        Parameters:
            _from (str): start date (YYYY-MM-DD)
            to (str): end date (YYYY-MM-DD)

        Returns:
            dict: data value contains a list of dictionaries, which contains the following keys:
            'date', 'epsActual', 'epsEstimate', 'hour', 'quarter', 'revenueActual', 'revenueEstimate', 'symbol', 'year'
        """
        response = self.fc.earnings_calendar(_from=_from, to=to, symbol=None)
        earnings_calendar = response.get('earningsCalendar')

        # return list elements in reverse order since response returns nearest earnings at the end
        reversed_earnings_calendar = earnings_calendar[::-1]

        return {"earnings_calendar": reversed_earnings_calendar}

    def get_upcoming_ipos(self, _from: str, to: str) -> dict[str, list]:
        """
        Get the anticipated IPOs (Initial Public Offering) for a specified date range.

        Parameters:
            _from (str): start date (YYYY-MM-DD)
            to (str): end date (YYYY-MM-DD)

        Returns:
            dict: data value contains a list of dictionaries, which contains the following keys:
            'date', 'exchange', 'name', 'numberOfShares', 'price', 'status', 'symbol', 'totalSharesValue'
        """
        response = self.fc.ipo_calendar(_from=_from, to=to)
        anticipated_ipos = response.get('ipoCalendar')

        # return list elements in reverse order since response returns nearest ipos at the end
        reversed_anticipated_ipos = anticipated_ipos[::-1]

        return reversed_anticipated_ipos
