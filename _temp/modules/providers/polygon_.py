import os
from datetime import datetime

import urllib3
from dotenv import load_dotenv
import pytz

import plotly.graph_objects as go
import plotly.io as pio
import plotly.subplots as subplots

from polygon import RESTClient

load_dotenv()


class Polygon:
    def __init__(
            self,
            ticker: str,
            multiplier: int,
            timespan: str,
            from_: str,
            to: str,
            limit: int
    ):
        """
        Initialize the Polygon class with the provided parameters.

        Parameters:
            ticker (str): The ticker symbol. For an options contract, the ticker will begin with an 'O:' indicating
                an option ticker. For example, '0:AAPL250620C00215000' reads as AAPL 2025-06-20 expiring Call for the
                strike price of $215.
            multiplier (int): The multiplier for the timespan.
            timespan (str): The timespan (minute, hour, day, week, month, quarter, year).
            from_ (str): The start date.
            to (str): The end date.
            limit (int): The number of bars to return; default 5000, max 50000.
        """
        self.api_key = os.getenv('POLYGON_API_KEY')
        self.polygon_client = RESTClient(api_key=self.api_key)
        self.ticker = ticker
        self.multiplier = multiplier
        self.timespan = timespan
        self.from_ = from_
        self.to = to
        self.limit = limit

    def make_chart(self) -> dict:
        """
        Get OHLCV bars for a given ticker. Along with OHLCV, get timestamp,
        number of transactions during that (multiplier*timespan) period, and volume weighted average price (vwap)
        For example, if multiplier=5 and timespan='minute', 5 minute bars will be returned.

        Extract each attribute from aggregate bar data.

        Convert timestamps from UTC to Pacific, and then convert those to a readable format of 'YYYY-MM-DD HH:MM:SS'

        Plot the candlestick chart of the ohlcv bars and volume chart with the volume bars. Create three rows (subplots)

        Returns:
            dict: success/failure message, information containing error message if rate limit is hit and the except clause is executed
        """
        # if rate limit has not been hit yet
        try:
            bars = []
            for agg in self.polygon_client.list_aggs(
                    ticker=self.ticker,
                    multiplier=self.multiplier,
                    timespan=self.timespan,
                    from_=self.from_,
                    to=self.to,
                    limit=self.limit
            ):
                bars.append(agg)

            # extract each attribute from aggregate bars
            timestamps = [bar.timestamp for bar in bars]
            opens = [bar.open for bar in bars]
            highs = [bar.high for bar in bars]
            lows = [bar.low for bar in bars]
            closes = [bar.close for bar in bars]
            volumes = [bar.volume for bar in bars]
            number_of_transactions = [bar.transactions for bar in bars]

            attributes = timestamps, opens, highs, lows, closes, volumes, number_of_transactions

            # list of timestamps is the first element in the attributes tuple
            timestamps = attributes[0]

            # define timezones
            utc_zone = pytz.utc
            pst_zone = pytz.timezone('US/Pacific')

            # convert timestamps to datetime objects in UTC
            datetime_utc = [datetime.utcfromtimestamp(ts / 1000).replace(tzinfo=utc_zone) for ts in timestamps]

            # convert UTC datetime to PST
            datetime_pst = [dt_utc.astimezone(pst_zone) for dt_utc in datetime_utc]

            # extract datetime strings for plotting
            datetime_strings = [dt_pst.strftime('%Y-%m-%d %H:%M:%S') for dt_pst in datetime_pst]

            # make subplot of two rows to plot two graphs sharing an x-axis, the timestamps.
            fig = subplots.make_subplots(
                rows=3,
                cols=1,
                shared_xaxes=True,
                vertical_spacing=0.02,
                row_heights=[0.6, 0.25, 0.15]
            )

            # add candlestick trace to row one of subplot
            fig.add_trace(
                go.Candlestick(
                    x=datetime_strings,
                    open=opens,
                    high=highs,
                    low=lows,
                    close=closes,
                    name='Price'
                ),
                row=1,
                col=1
            )

            # add volume bar trace to row two of subplot
            fig.add_trace(
                go.Bar(
                    x=datetime_strings,
                    y=volumes,
                    name='Volume',
                    yaxis='y2'
                ),
                row=2,
                col=1
            )

            # add number of transactions bar trace to row three of subplot
            fig.add_trace(
                go.Bar(
                    x=datetime_strings,
                    y=number_of_transactions,
                    name='Transactions',
                    yaxis='y3'
                ),
                row=3,
                col=1
            )

            # update layout
            fig.update_layout(
                yaxis1_title='Price',
                yaxis2_title='Volume',
                yaxis3_title='Transactions',
                xaxis1_title='Time',
                xaxis_rangeslider_visible=False,
                width=1300,
                height=700,
                margin=dict(
                    l=10,
                    r=10,
                    t=10,
                    b=10
                ),
            )

            # get rid of gaps from weekend/holiday closures and non-market hours
            fig.update_xaxes(type='category')

            # convert plot to HTML string
            plot_html = pio.to_html(fig, full_html=False)

            return {
                'success': True,
                'data': plot_html
            }

        except urllib3.exceptions.MaxRetryError:
            return {
                'success': False,
                'data': "Chart not available. The overall rate limit for now is 5 charts per minute, I am working to fix this!"
            }
