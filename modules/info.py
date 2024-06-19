import yfinance as yf
from pandas import Timestamp

from modules.providers.yfinance_ import YFinance
from modules.providers.finnhub_ import Finnhub
from modules.providers.alphavantage import AlphaVantage
from modules.providers.federalreserve import FederalReserve
from modules.providers.tradier import Tradier
from modules.providers.polygon_ import Polygon


y = YFinance('SOFI')
f = Finnhub()
a = AlphaVantage()
fred = FederalReserve()
t = Tradier()
polygon = Polygon(
        ticker='SOFI',
        multiplier=1,
        timespan='hour',
        from_='2024-06-14',
        to='2024-06-18',
        limit=50000
    )


sofi = yf.Ticker('SOFI')
# print(sofi.get_cashflow(freq='quarterly'))
# print(sofi.get_incomestmt(freq='quarterly'))
# print(sofi.get_balance_sheet(freq='quarterly'))

# print(type(fred.get_fdi().get(Timestamp('1946-10-01 00:00:00'))))
