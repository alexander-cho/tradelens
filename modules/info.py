import yfinance as yf
from pandas import Timestamp
from datetime import datetime

from modules.providers.yfinance_ import YFinance
from modules.providers.finnhub_ import Finnhub
from modules.providers.alphavantage import AlphaVantage
from modules.providers.federalreserve import FederalReserve
from modules.providers.tradier import Tradier
from modules.providers.polygon_ import Polygon
from modules.providers.fmp import FMP

from modules.max_pain import MaxPain


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
