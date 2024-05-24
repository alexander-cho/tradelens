import finnhub


API_KEY = "cp42qm9r01qs36663pfgcp42qm9r01qs36663pg0"
finnhub_client = finnhub.Client(api_key=API_KEY)


class Finnhub:
    def __init__(self):
        self.api_key = API_KEY
        self.fc = finnhub_client

    def get_market_status(self) -> dict:
        """
        Get the current market status for US exchanges

        Returns:
            dict: market status
        """
        market_status = self.fc.market_status(exchange='US')
        return market_status

    def get_market_news(self) -> list[dict]:
        """
        Get overall market news

        Returns:
            list: List of dictionaries each containing info for news article
        """
        general_news = self.fc.general_news(category='general', min_id=0)
        return general_news

    def get_stock_news(self, ticker, _from, to) -> list[dict]:
        """
        Get recent news for a specific stock

        Args:
            ticker (str): ticker symbol
            _from (str): start date
            to (str): end date

        Returns:
            list: list of dictionaries, each containing news data from an external article
            keys: ['category', 'datetime', 'headline', 'id', 'image', 'related', 'source', 'summary', 'url']
        """
        ticker_news = self.fc.company_news(symbol=ticker, _from=_from, to=to)
        return ticker_news

    def get_company_profile(self, ticker) -> dict:
        """
        Get the basic profile data for a company, and return certain keys of use

        Args:
            ticker (str): ticker symbol

        Returns:
            dict: basic profile data, which includes the web URL and company logo
        """
        company_profile = self.fc.company_profile2(symbol=ticker)
        keys = ['ticker', 'ipo', 'weburl', 'logo', 'finnhubIndustry']
        condensed_profile = {}

        for key in keys:
            if key in company_profile:
                condensed_profile[key] = company_profile[key]

        return condensed_profile

    def get_insider_sentiment(self, ticker, _from, to) -> dict:
        """
        Get the insider sentiment data for a specific stock, based on monthly purchases.
        Metric called MSPR (monthly share purchase ratio) is used, if market purchases > market sells, sentiment
        is considered positive.

        The closer this number is to 100 (-100) the more reliable that the stock prices of the firm
        increase (decrease) in the next periods.

        Args:
            ticker (str): ticker symbol
            _from (str): start date
            to (str): end date

        Returns:
            dict containing keys such as "change" which refers to the net buying/selling of insiders in number of shares
            "mspr": monthly share purchase ratio
        """
        insider_sentiment = self.fc.stock_insider_sentiment(symbol=ticker, _from=_from, to=to)
        return insider_sentiment
