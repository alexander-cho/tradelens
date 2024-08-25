import os
from datetime import datetime
import pytz

from dotenv import load_dotenv

import plotly.graph_objects as go
import plotly.io as pio

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
        market_status = self.fc.market_status(exchange='US')

        # Get the current time in PST
        pst_tz = pytz.timezone('America/Los_Angeles')
        current_pst_time = datetime.now(pst_tz)

        # Insert the current PST time and timezone into the market_status dictionary
        market_status['t'] = current_pst_time.strftime('%Y-%m-%d %H:%M:%S')
        market_status['timezone'] = 'America/Los_Angeles'

        return {
            'description': 'market_status',
            'data': market_status
        }

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

    def get_market_news(self, category: str) -> dict:
        """
        Get overall market news

        Returns:
            list: List of dictionaries each containing info for news article
        """
        market_news = self.fc.general_news(category=category, min_id=0)

        # convert unix timestamp to pst time
        for article in market_news:
            datetime_to_utc = datetime.utcfromtimestamp(article['datetime']).replace(tzinfo=pytz.utc)
            pst_tz = pytz.timezone('America/Los_Angeles')
            article_datetime_pst = datetime_to_utc.astimezone(pst_tz)
            # replace with PST
            article['datetime'] = article_datetime_pst.strftime('%Y-%m-%d %H:%M:%S')

        return {
            'description': 'market_news',
            'category': category,
            'data': market_news
        }

    def get_stock_news(self, ticker: str, _from: str, to: str) -> dict:
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
        date_range = {'from': _from, 'to': to}

        # convert unix timestamp to pst time
        for article in ticker_news:
            datetime_to_utc = datetime.utcfromtimestamp(article['datetime']).replace(tzinfo=pytz.utc)
            pst_tz = pytz.timezone('America/Los_Angeles')
            article_datetime_pst = datetime_to_utc.astimezone(pst_tz)
            # replace with PST
            article['datetime'] = article_datetime_pst.strftime('%Y-%m-%d %H:%M:%S')

        return {
            'description': 'stock_news',
            'ticker': ticker,
            'date_range': date_range,
            'data': ticker_news
        }

    def get_company_profile(self, ticker: str) -> dict:
        """
        Get the basic profile data for a company, and return certain keys of use

        Parameters:
            ticker (str): ticker symbol

        Returns:
            dict: basic profile data, which includes the web URL and company logo
        """
        company_profile = self.fc.company_profile2(symbol=ticker)
        keys = [
            'ticker',
            'ipo',
            'weburl',
            'logo',
            'finnhubIndustry'
        ]
        condensed_profile = {}

        for key in keys:
            if key in company_profile:
                condensed_profile[key] = company_profile[key]

        return {
            'description': 'company_profile',
            'data': condensed_profile
        }

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
        insider_sentiment = self.fc.stock_insider_sentiment(symbol=ticker, _from=_from, to=to).get('data')
        date_range = {'from': _from, 'to': to}

        # original data has keys 'year' and 'month' as an integer; change to '<MONTH> <YEAR>'
        for sentiment in insider_sentiment:
            timestamp = datetime(year=sentiment['year'], month=sentiment['month'], day=1).strftime('%B %Y')
            sentiment['timestamp'] = timestamp

            # remove original k, v
            del sentiment['year']
            del sentiment['month']

        return {
            'description': 'insider_sentiment',
            'ticker': ticker,
            'date_range': date_range,
            'data': insider_sentiment
        }

    def get_insider_transactions(self, ticker: str, _from: str, to: str) -> dict:
        insider_transactions = self.fc.stock_insider_transactions(symbol=ticker, _from=_from, to=to).get('data')
        date_range = {'from': _from, 'to': to}

        insider_transactions_reversed = insider_transactions[::-1]

        return {
            'description': 'insider_transactions',
            'ticker': ticker,
            'date_range': date_range,
            'data': insider_transactions_reversed
        }

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

        date_range = {'from': _from, 'to': to}
        lobbying_activities_reversed = filtered_data[::-1]

        return {
            'description': 'lobbying_activities',
            'symbol': ticker,
            'date_range': date_range,
            'data': lobbying_activities_reversed
        }

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
                "awardingSubAgencyName": activity.get("awardingSubAgencyName"),
                "awardingOfficeName": activity.get("awardingOfficeName"),
                "awardDescription": activity.get("awardDescription"),
                "permalink": activity.get("permalink")
            }
            filtered_data.append(filtered_activity)

        date_range = {'from': _from, 'to': to}

        return {
            'description': 'government_spending',
            'symbol': ticker,
            'date_range': date_range,
            'data': filtered_data
        }

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
        date_range = {'from': _from, 'to': to}
        earnings_calendar = response.get('earningsCalendar')

        # return list elements in reverse order since response returns nearest earnings at the end
        reversed_earnings_calendar = earnings_calendar[::-1]

        # remove the scheduled earnings if a reporting hour is not present
        contains_hour = [earning for earning in reversed_earnings_calendar if earning['hour'] != '']

        # change values amc and bmo to be more descriptive
        for earning in contains_hour:
            if earning['hour'] == 'bmo':
                earning['hour'] = 'Before market open'
            elif earning['hour'] == 'amc':
                earning['hour'] = 'After market close'

            # Add "Q" in front of the quarter value
            earning['quarter'] = 'Q' + str(earning['quarter'])

        final_calendar = contains_hour

        return {
            'description': 'market_wide_earnings_calendar',
            'date_range': date_range,
            'data': final_calendar
        }

    def get_upcoming_ipos(self, _from: str, to: str) -> dict:
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
        date_range = {'from': _from, 'to': to}
        anticipated_ipos = response.get('ipoCalendar')

        # return list elements in reverse order since response returns nearest ipos at the end
        reversed_anticipated_ipos = anticipated_ipos[::-1]

        return {
            'description': 'market_wide_ipos',
            'date_range': date_range,
            'data': reversed_anticipated_ipos
        }

    def plot_insider_sentiment(self, ticker: str, _from: str, to: str) -> str:
        insider_sentiment = self.get_insider_sentiment(ticker=ticker, _from=_from, to=to).get('data')
        timestamp = [sentiment['timestamp'] for sentiment in insider_sentiment]
        change = [sentiment['change'] for sentiment in insider_sentiment]

        # Set colors: green for positive change, red for negative change
        colors = ['green' if c > 0 else 'red' for c in change]

        # hover popups
        mspr = [sentiment['mspr'] for sentiment in insider_sentiment]

        trace = go.Bar(
            x=timestamp,
            y=change,
            marker={'color': colors},
            hovertemplate='<b>%{x}</b><br>'
                          'Shares: %{y}<br>'
                          'MSPR: %{customdata}<extra></extra>',
            customdata=list(mspr)
        )

        fig = go.Figure(data=trace)

        # update layout for bar chart
        fig.update_layout(
            title='Insider sentiment based on Monthly Share Purchase Ratio',
            xaxis_title='Month',
            yaxis_title='Net buying/selling from all insider transactions.',
            height=700
        )

        plot_html = pio.to_html(fig, full_html=False)
        return plot_html

    def plot_insider_transactions(self, ticker: str, _from: str, to: str) -> str:
        insider_transactions = self.get_insider_transactions(ticker=ticker, _from=_from, to=to).get('data')
        transaction_date = [transaction['transactionDate'] for transaction in insider_transactions]
        change = [transaction['change'] for transaction in insider_transactions]

        # Set colors: green for positive change, red for negative change
        colors = ['green' if c > 0 else 'red' for c in change]

        # hover popups
        insider_name = [transaction['name'] for transaction in insider_transactions]
        shares_remaining = [transaction['share'] for transaction in insider_transactions]
        transaction_price = [transaction['transactionPrice'] for transaction in insider_transactions]
        is_derivative = [transaction['isDerivative'] for transaction in insider_transactions]
        transaction_code = [transaction['transactionCode'] for transaction in insider_transactions]

        trace = go.Bar(
            x=transaction_date,
            y=change,
            marker={'color': colors},
            hovertemplate='<b>%{x}</b><br>'
                          '<b>Name:</b> %{customdata[0]}<br>'
                          '<b>Change in shares:</b> %{y}<br>'
                          '<b>Shares remaining:</b> %{customdata[1]}<br>'
                          '<b>Price per share:</b> %{customdata[2]}<br>'
                          '<b>Options exercise?</b> %{customdata[3]}<br>'
                          '<b>Transaction code:</b> %{customdata[4]}<extra></extra>',
            customdata=list(zip(insider_name, shares_remaining, transaction_price, is_derivative, transaction_code))
        )

        fig = go.Figure(data=trace)

        # update layout for bar chart
        fig.update_layout(
            title='Insider Transactions',
            xaxis_title='Date',
            yaxis_title='Shares',
            height=700
        )

        plot_html = pio.to_html(fig, full_html=False)
        return plot_html
