import os
from dotenv import load_dotenv
import requests

load_dotenv()


class Tradier:
    """
    Class containing methods for fetching data from the Tradier brokerage API
    """
    def __init__(self):
        self.api_key = os.getenv('Tradier_API_KEY')
        self.options_chain_url = 'https://api.tradier.com/v1/markets/options/chains'

    def get_options_chain(self, symbol, expiration_date) -> list[dict[str, any]]:
        """
        Get options chain data for a specific expiration date of a particular underlying

        Parameters:
            symbol (str): the symbol of the options chain
            expiration_date (str): the expiration date of the options chain

        Returns:
            dict: json response containing comprehensive options chain data including the greeks, for each contract
        """
        response = requests.get(f"{self.options_chain_url}",
                                params={'symbol': f'{symbol}', 'expiration': f'{expiration_date}', 'greeks': 'true'},
                                headers={'Authorization': 'Bearer JgHNiQ4kHjnloj3HxVLb63EDoevk',
                                         'Accept': 'application/json'}
                                )
        response_to_json = response.json()

        # access nested dictionary and extract list of dictionaries containing the data
        options_chain = response_to_json.get('options', {}).get('option', [])
        return options_chain
