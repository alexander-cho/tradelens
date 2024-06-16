import os
from dotenv import load_dotenv
from datetime import datetime
import pytz

import plotly.graph_objects as go
import plotly.subplots as subplots

from polygon import RESTClient


load_dotenv()

api_key = os.getenv('POLYGON_API_KEY')

client = RESTClient(api_key)


ticker = 'SOFI'
multiplier = 1
timespan = 'hour'
from_ = '2024-06-10'
to = '2024-06-14'
limit = 50000


bars = []

for a in client.list_aggs(
    ticker=ticker,
    multiplier=multiplier,
    timespan=timespan,
    from_=from_,
    to=to,
    limit=limit
):
    bars.append(a)


# Define timezones
utc_zone = pytz.utc
pst_zone = pytz.timezone('US/Pacific')


# extract each attribute from aggregate bars
timestamps = [bar.timestamp for bar in bars]
opens = [bar.open for bar in bars]
highs = [bar.high for bar in bars]
lows = [bar.low for bar in bars]
closes = [bar.close for bar in bars]
volumes = [bar.volume for bar in bars]
number_of_transactions = [bar.transactions for bar in bars]


# Convert timestamps to datetime objects in UTC
datetime_utc = [datetime.utcfromtimestamp(ts / 1000).replace(tzinfo=utc_zone) for ts in timestamps]

# Convert UTC datetime to PST
datetime_pst = [dt_utc.astimezone(pst_zone) for dt_utc in datetime_utc]

# Extract datetime strings for plotting
datetime_strings = [dt_pst.strftime('%Y-%m-%d %H:%M:%S') for dt_pst in datetime_pst]


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


# Update the layout for better visualization
fig.update_layout(
    title=f'Chart for {ticker} ({multiplier} {timespan})',
    yaxis1_title='Share price',
    yaxis2_title='Volume (M)',
    xaxis1_title='Time',
    xaxis_rangeslider_visible=False
)


# get rid of weekend/holiday market closure gaps
fig.layout.xaxis.type = 'category'


fig.show()

# print(bars)
