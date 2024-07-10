import yfinance as yf
import talib


sofi = yf.download('SOFI', start='2023-07-09', end='2024-07-09')

# morning star
spy_morning_stars = talib.CDLMORNINGSTAR(
    open=sofi['Open'],
    high=sofi['High'],
    low=sofi['Low'],
    close=sofi['Close']
)

# engulfing
spy_engulfing = talib.CDLENGULFING(
    open=sofi['Open'],
    high=sofi['High'],
    low=sofi['Low'],
    close=sofi['Close']
)

sofi['morning_star'] = spy_morning_stars
sofi['engulfing'] = spy_engulfing

print(sofi[sofi['engulfing'] != 0])
