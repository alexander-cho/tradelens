import yfinance as yf

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
print(sofi.get_cashflow(freq='quarterly'))
print(sofi.get_incomestmt(freq='quarterly'))
print(sofi.get_balance_sheet(freq='quarterly'))
