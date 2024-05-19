import finnhub


API_KEY = "cp42qm9r01qs36663pfgcp42qm9r01qs36663pg0"
finnhub_client = finnhub.Client(api_key=API_KEY)


class Finnhub:
    def __init__(self):
        self.api_key = API_KEY
        self.fc = finnhub_client

    def get_market_news(self):
        """
        Get overall market news

        Returns:
        """
        return self.fc.general_news('general', min_id=0)

    def get_stock_news(self, stock, _from, to):
        """
        Get recent news for a specific stock

        Args:
            stock (str): stock name
            _from (str): start date
            to (str): end date

        Returns:
            list of dictionaries, each containing news data from an external article
            keys: ['category', 'datetime', 'headline', 'id', 'image', 'related', 'source', 'summary', 'url']
        """
        return self.fc.company_news(stock, _from, to)
