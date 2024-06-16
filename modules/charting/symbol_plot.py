from modules.providers.polygon_ import Polygon


polygon = Polygon()
print(polygon.get_ohlcv_bars(
        ticker='SOFI',
        multiplier=5,
        timespan='minute',
        from_='2024-06-10',
        to='2024-06-14',
        limit=50000
    )
)
