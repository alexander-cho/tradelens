import os
from dotenv import load_dotenv
import requests
import warnings

warnings.filterwarnings(action="ignore", category=FutureWarning)

load_dotenv()


class AlphaVantage:
    def __init__(self):
        self.api_key = os.getenv('ALPHAVANTAGE_API_KEY')
        self.TOP_GAINERS_LOSERS_URL = f'https://www.alphavantage.co/query?function=TOP_GAINERS_LOSERS&apikey={self.api_key}'

    def get_top_gainers_losers(self) -> dict:
        """
        Get the 20 top gainers and losers, as well as the most actively traded stocks for the day

        Returns:
            dict: with keys ['metadata', 'last_updated', 'top_gainers', 'top_losers', 'most_actively_traded']

                'top_gainers', 'top_losers', 'most_actively_trade':
                    list of dictionaries for each ticker containing the following keys
                    ['ticker', 'price', 'change_amount', 'change_percentage', 'volume']

        https://realpython.com/caching-external-api-requests/
        """
        request = requests.get(self.TOP_GAINERS_LOSERS_URL)
        top_gainers_losers = request.json()
        return top_gainers_losers
