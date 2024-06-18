import os
from datetime import datetime
from dotenv import load_dotenv
import pandas as pd
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
            ticker (str): The ticker symbol.
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

    def _get_ohlcv_bars(self) -> list:
        """
        Get OHLCV bars for a given ticker. Along with OHLCV, get timestamp,
        number of transactions during that (multiplier*timespan) period, and volume weighted average price (vwap)
        For example, if multiplier=5 and timespan='minute', 5 minute bars will be returned.

        Returns:
            list: list of Agg model objects with the following attributes
            open, high, low, close, volume, vwap, timestamp, transactions, otc
        """
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

        return bars

    def _bars_to_df_(self) -> pd.DataFrame:
        """
        Get ohlcv+ aggregate information into as a dataframe, with index set to the timestamp.

        Returns:
            pd.DataFrame: OHLCV+ aggregate information into as a dataframe.
        """
        ohlcv_bars_df = pd.DataFrame(self._get_ohlcv_bars()).set_index('timestamp')
        return ohlcv_bars_df

    def _extract_attributes(self) -> tuple:
        """
        Extract each attribute from aggregate bar data.

        Returns:
            tuple: containing lists for each attribute (data points per timestamp)
        """
        bars_to_extract = self._get_ohlcv_bars()

        # extract each attribute from aggregate bars
        timestamps = [bar.timestamp for bar in bars_to_extract]
        opens = [bar.open for bar in bars_to_extract]
        highs = [bar.high for bar in bars_to_extract]
        lows = [bar.low for bar in bars_to_extract]
        closes = [bar.close for bar in bars_to_extract]
        volumes = [bar.volume for bar in bars_to_extract]
        number_of_transactions = [bar.transactions for bar in bars_to_extract]

        attributes = timestamps, opens, highs, lows, closes, volumes, number_of_transactions
        return attributes

    def _get_datetime_strings(self) -> list:
        """
        Convert timestamps from UTC to Pacific, and then convert those to a readable format of 'YYYY-MM-DD HH:MM:SS'

        Returns:
            list: list of datetime strings
        """
        # list of timestamps is the first element in the attributes tuple
        timestamps = self._extract_attributes()[0]

        # Define timezones
        utc_zone = pytz.utc
        pst_zone = pytz.timezone('US/Pacific')

        # Convert timestamps to datetime objects in UTC
        datetime_utc = [datetime.utcfromtimestamp(ts / 1000).replace(tzinfo=utc_zone) for ts in timestamps]

        # Convert UTC datetime to PST
        datetime_pst = [dt_utc.astimezone(pst_zone) for dt_utc in datetime_utc]

        # Extract datetime strings for plotting
        datetime_strings = [dt_pst.strftime('%Y-%m-%d %H:%M:%S') for dt_pst in datetime_pst]

        return datetime_strings

    def create_symbol_chart(self) -> str:
        """
        Plot the candlestick chart of the ohlcv bars and volume chart with the volume bars. Create two rows (subplots)

        Returns:
            str: symbol plot as a string to render as a html div
        """
        # extract attributes
        timestamps, opens, highs, lows, closes, volumes, number_of_transactions = self._extract_attributes()
        datetime_strings = self._get_datetime_strings()

        # make subplot of two rows to plot two graphs sharing an x-axis, the timestamps.
        fig = subplots.make_subplots(
            rows=2,
            cols=1,
            shared_xaxes=True,
            vertical_spacing=0.02,
            row_heights=[0.7, 0.3]
        )

        # Add candlestick trace to row one of subplot
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

        # Add volume bar trace to row two of subplot
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

        # update layout
        fig.update_layout(
            yaxis1_title='Share price',
            yaxis2_title='Volume (M)',
            xaxis1_title='Time',
            xaxis_rangeslider_visible=False,
            width=1300,
            height=700,
            margin=dict(
                l=10,
                r=10,
                t=10,
                b=10
            )
        )

        # get rid of gaps from weekend/holiday closures and non-market hours
        fig.update_xaxes(type='category')

        # Convert plot to HTML string
        plot_html = pio.to_html(fig, full_html=False)

        return plot_html
