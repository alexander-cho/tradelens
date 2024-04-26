import yfinance as yf

ticker = yf.Ticker('SOFI')


analyst_info = ticker.get_upgrades_downgrades()
print(analyst_info)
