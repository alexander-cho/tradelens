import yfinance as yf
from pandas import Timestamp

from modules.providers.yfinance_ import YFinance
from modules.providers.finnhub_ import Finnhub
from modules.providers.alphavantage import AlphaVantage
from modules.providers.federalreserve import FederalReserve
from modules.providers.tradier import Tradier
from modules.providers.polygon_ import Polygon
from modules.providers.fmp import FMP


y = YFinance('SOFI')
f = Finnhub()
a = AlphaVantage()
fred = FederalReserve()
t = Tradier()
polygon_ticker = Polygon(
        ticker='SOFI',
        multiplier=1,
        timespan='hour',
        from_='2024-06-14',
        to='2024-06-18',
        limit=50000
)
polygon_option = Polygon(
        ticker='O:SOFI240719C00008000',
        multiplier=1,
        timespan='hour',
        from_='2024-06-17',
        to='2024-06-21',
        limit=50000
)
fmp = FMP()

option_bars = polygon_option._get_ohlcv_bars()
for bar in option_bars:
    print(bar)
