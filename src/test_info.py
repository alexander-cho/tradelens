from _yfinance import YFinance
from _finnhub import Finnhub
from _alphavantage import AlphaVantage
from _federalreserve import FederalReserve
from _tradier import Tradier


y = YFinance('SOFI')
f = Finnhub()
a = AlphaVantage()
fred = FederalReserve()
t = Tradier()


import finnhub
finnhub_client = finnhub.Client(api_key="cp42qm9r01qs36663pfgcp42qm9r01qs36663pg0")

for i in finnhub_client.ipo_calendar(_from="2024-06-01", to="2029-12-31").get('ipoCalendar')[::-1]:
    print(i)

