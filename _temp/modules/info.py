import yfinance as yf
from pandas import Timestamp
from datetime import datetime

from providers.yfinance_ import YFinance
from providers.finnhub_ import Finnhub
from providers.alphavantage import AlphaVantage
from providers.federalreserve import FederalReserve
from providers.tradier import Tradier
from providers.polygon_ import Polygon
from providers.fmp import FMP

from max_pain import MaxPain


y = YFinance('SOFI')
f = Finnhub()
a = AlphaVantage()
fred = FederalReserve()
t = Tradier('SOFI')
polygon_ticker = Polygon(
        ticker='SOFI',
        multiplier=1,
        timespan='hour',
        from_='2024-06-14',
        to='2024-06-18',
        limit=50000
)
polygon_option = Polygon(
        ticker='O:SOFI240705C00005000',
        multiplier=1,
        timespan='hour',
        from_='2024-05-23',
        to='2024-06-21',
        limit=50000
)
fmp = FMP()
max_pain = MaxPain('SOFI')

# for k, v in y.get_calendar().get('data').items():
#     print(type(v))

# print(y.get_options_expiry_list())
