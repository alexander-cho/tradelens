from src.providers.yfinance_ import YFinance
from src.providers.finnhub import Finnhub
from src.providers.alphavantage import AlphaVantage
from src.providers.federalreserve import FederalReserve
from src.providers.tradier import Tradier


y = YFinance('SOFI')
f = Finnhub()
a = AlphaVantage()
fred = FederalReserve()
t = Tradier()
