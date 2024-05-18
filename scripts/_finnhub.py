import finnhub


API_KEY = "cp42qm9r01qs36663pfgcp42qm9r01qs36663pg0"
finnhub_client = finnhub.Client(api_key=API_KEY)


class Finnhub:
    def __init__(self):
        self.api_key = API_KEY
        self.fc = finnhub_client

    def get_market_news(self):
        return self.fc.general_news('general', min_id=0)

