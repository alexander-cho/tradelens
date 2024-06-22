import os
from dotenv import load_dotenv
import requests

load_dotenv()


class Tradier:
    """
    Class containing methods for fetching data from the Tradier brokerage API
    """
    def __init__(self, symbol):
        self.api_key = os.getenv('TRADIER_API_KEY')
        self.options_chain_url = 'https://api.tradier.com/v1/markets/options/chains'
        self.symbol = symbol

    def get_options_chain(self, expiration_date: str) -> list[dict[str, any]]:
        """
        Get options chain data for a specific expiration date of a particular underlying

        Parameters:
            expiration_date (str): the expiration date of the options chain

        Returns:
            dict: json response containing comprehensive options chain data including the greeks, for each contract
        """
        response = requests.get(f"{self.options_chain_url}",
                                params={'symbol': f'{self.symbol}', 'expiration': f'{expiration_date}', 'greeks': 'true'},
                                headers={'Authorization': 'Bearer JgHNiQ4kHjnloj3HxVLb63EDoevk',
                                         'Accept': 'application/json'}
                                )
        response_to_json = response.json()

        # access nested dictionary and extract list of dictionaries containing the data
        options_chain = response_to_json.get('options', {}).get('option', [])
        return options_chain

    def _get_open_interest(self, expiration_date: str) -> dict:
        """
        Get the open interest of each strike for a specific expiration date

        Parameters:
             expiration_date (str): expiration date of format 'YYYY-MM-DD'

        Returns:
            dict: response containing keys including 'data', containing the open interest information
        """
        # initialize to populate with corresponding data
        call_data = {}
        put_data = {}

        # get the option chain for the desired expiration date
        options_chain = self.get_options_chain(expiration_date)

        # within the defined option chain response data, if the key 'option_type' has a value of 'call', get the
        # value of the 'open_interest' key and populate the call_data dictionary defined earlier. Same for the puts.
        for strike in options_chain:
            if strike['option_type'] == 'call':
                call_data[strike['strike']] = strike['open_interest']
            elif strike['option_type'] == 'put':
                put_data[strike['strike']] = strike['open_interest']

        # create a list of dictionaries, each defining the strike and the corresponding open interest
        call_list = [{'strike': strike, 'open_interest': open_interest} for strike, open_interest in call_data.items()]
        put_list = [{'strike': strike, 'open_interest': open_interest} for strike, open_interest in put_data.items()]

        # return the response
        return {
            'root_symbol': self.symbol,
            'expiration_date': expiration_date,
            'data': {"Calls": call_list, "Puts": put_list}
        }

    def _get_volume(self, expiration_date: str):
        """
        Get the volume of each strike for a specific expiration date

        Parameters:
             expiration_date (str): expiration date of format 'YYYY-MM-DD'

        Returns:
            dict: response containing keys including 'data', containing the open interest information
        """
        call_data = {}
        put_data = {}

        options_chain = self.get_options_chain(expiration_date)
        for strike in options_chain:
            if strike['option_type'] == 'call':
                call_data[strike['strike']] = strike['volume']
            elif strike['option_type'] == 'put':
                put_data[strike['strike']] = strike['volume']

        call_list = [{'strike': strike, 'volume': volume} for strike, volume in call_data.items()]
        put_list = [{'strike': strike, 'volume': volume} for strike, volume in put_data.items()]

        return {
            'root_symbol': self.symbol,
            'expiration_date': expiration_date,
            'data': {"Calls": call_list, "Puts": put_list}
        }

    def _get_implied_volatility(self, expiration_date: str):
        """
        Get the implied volatility measurement of each strike for a specific expiration date

        Parameters:
             expiration_date (str): expiration date of format 'YYYY-MM-DD'

        Returns:
            dict: response containing keys including 'data', containing the open interest information:
        """
        call_data = {}
        put_data = {}

        options_chain = self.get_options_chain(expiration_date)
        for strike in options_chain:
            if strike['option_type'] == 'call':
                call_data[strike['strike']] = strike['greeks'].get('mid_iv')
            elif strike['option_type'] == 'put':
                put_data[strike['strike']] = strike['greeks'].get('mid_iv')

        call_list = [{'strike': strike, 'iv': iv} for strike, iv in call_data.items()]
        put_list = [{'strike': strike, 'iv': iv} for strike, iv in put_data.items()]

        return {
            'root_symbol': self.symbol,
            'expiration_date': expiration_date,
            'data': {"Calls": call_list, "Puts": put_list}
        }

    def _get_last_bid_ask(self, expiration_date: str):
        """
        Get the last, bid, ask prices of each strike for a specific expiration date

        Parameters:
             expiration_date (str): expiration date of format 'YYYY-MM-DD'

        Returns:
            dict: response containing keys including 'data', containing the open interest information:
        """
        call_data = {}
        put_data = {}

        options_chain = self.get_options_chain(expiration_date)
        for strike in options_chain:
            if strike['option_type'] == 'call':
                call_data[strike['strike']] = {'last': strike['last'], 'bid': strike['bid'], 'ask': strike['ask']}
            elif strike['option_type'] == 'put':
                put_data[strike['strike']] = {'last': strike['last'], 'bid': strike['bid'], 'ask': strike['ask']}

        call_list = [{'strike': strike, 'last_bid_ask': last_bid_ask} for strike, last_bid_ask in call_data.items()]
        put_list = [{'strike': strike, 'last_bid_ask': last_bid_ask} for strike, last_bid_ask in put_data.items()]

        return {
            'root_symbol': self.symbol,
            'expiration_date': expiration_date,
            'data': {"Calls": call_list, "Puts": put_list}
        }
