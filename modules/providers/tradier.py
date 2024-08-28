import os
from dotenv import load_dotenv
import requests
import pandas as pd

import plotly.graph_objects as go
import plotly.subplots as subplots
import plotly.io as pio

load_dotenv()


class Tradier:
    """
    Class containing methods for fetching data from the Tradier brokerage API
    """
    def __init__(self, symbol):
        self.api_key = os.getenv('TRADIER_API_KEY')
        self.options_chain_url = 'https://api.tradier.com/v1/markets/options/chains'
        self.quote_url = 'https://api.tradier.com/v1/markets/quotes'
        self.symbol = symbol

    def get_options_chain(self, expiration_date: str) -> dict:
        """
        Get options chain data for a specific expiration date of a particular underlying

        Parameters:
            expiration_date (str): the expiration date of the options chain formatted as YYYY-MM-DD

        Returns:
            dict: json response containing comprehensive options chain data including the greeks, for each contract
        """
        response = requests.get(
            self.options_chain_url,
            params={
                'symbol': f'{self.symbol}',
                'expiration': f'{expiration_date}',
                'greeks': 'true'
            },
            headers={
                'Authorization': f'Bearer {self.api_key}',
                'Accept': 'application/json'
            })
        response_to_json = response.json()

        # access nested dictionary and extract list of dictionaries containing the options chain
        options_chain = response_to_json.get('options', {}).get('option', [])

        return {
            'description': 'full option chain for a given expiration date',
            'ticker': self.symbol,
            'expiration_date': expiration_date,
            'data': options_chain
        }

    def get_strikes(self, expiration_date: str) -> dict:
        """
        Get the options tickers for each strike price of a particular expiration date

        Parameters:
            expiration_date (str): the expiration date of the options chain formatted as YYYY-MM-DD

        Returns:
            dict: response containing keys such as 'data', which maps to a value which is a dictionary containing
                key value pairs of the description and the option ticker
        """
        options_chain = self.get_options_chain(expiration_date).get('data')

        call_strikes = {}
        put_strikes = {}

        # for each strike dictionary in the options chain list, if the value of mapping from the 'description' key
        # contains 'Call' or 'Put'
        for strike in options_chain:
            if 'Call' in strike.get('description'):
                call_strikes[strike['description']] = strike['symbol']
            else:
                put_strikes[strike['description']] = strike['symbol']

        return {
            'description': 'list of strike prices for a chain',
            'root_symbol': self.symbol,
            'expiration_date': expiration_date,
            'data': {
                'calls': call_strikes,
                'puts': put_strikes
            }
        }

    def get_open_interest(self, expiration_date: str) -> dict:
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
        options_chain = self.get_options_chain(expiration_date=expiration_date).get('data')

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

        # calculate the total open interest for calls and puts for a chain
        total_call_oi = 0
        total_put_oi = 0

        for call, put in zip(call_list, put_list):
            call_oi = call.get('open_interest', 0)
            put_oi = put.get('open_interest', 0)
            total_call_oi += call_oi
            total_put_oi += put_oi

        # get the put call ratio
        put_call_ratio = total_put_oi / total_call_oi

        # return the response
        return {
            'description': 'open_interest',
            'root_symbol': self.symbol,
            'expiration_date': expiration_date,
            'data': {
                "Calls": call_list,
                "Puts": put_list,
                "total_call_oi": total_call_oi,
                "total_put_oi": total_put_oi,
                "put_call_ratio": put_call_ratio
            }
        }

    def plot_open_interest(self, expiration_date: str) -> str:
        """
        Plot open interest chart for calls and puts; 2 traces (bars)
        """
        open_interest = self.get_open_interest(expiration_date=expiration_date)
        calls = open_interest.get('data', {}).get('Calls', [])
        puts = open_interest.get('data', {}).get('Puts', [])

        total_call_oi = open_interest.get('data', {}).get('total_call_oi', 0)
        total_put_oi = open_interest.get('data', {}).get('total_put_oi', 0)
        put_call_ratio = open_interest.get('data', {}).get('put_call_ratio', 0)

        # convert lists to DataFrames
        df_calls = pd.DataFrame(calls)
        df_puts = pd.DataFrame(puts)

        trace_calls = go.Bar(
            x=df_calls['strike'],
            y=df_calls['open_interest'],
            name='Calls',
            marker={
                'color': 'green'
            }
        )
        trace_puts = go.Bar(
            x=df_puts['strike'],
            y=df_puts['open_interest'],
            name='Puts',
            marker={
                'color': 'red'
            }
        )

        fig = go.Figure(data=[trace_calls, trace_puts])

        # update layout for stacked bar mode
        fig.update_layout(
            barmode='stack',
            title=f'Open Interest | Total Call OI: {total_call_oi} | Total Put OI: {total_put_oi} | Put/Call Ratio: {put_call_ratio}',
            xaxis_title='Strike',
            yaxis_title='Open Interest',
            height=600,
        )

        plot_html = pio.to_html(fig, full_html=False)
        return plot_html

    def _get_volume(self, expiration_date: str) -> dict:
        """
        Get the volume of each strike for a specific expiration date

        Parameters:
             expiration_date (str): expiration date of format 'YYYY-MM-DD'

        Returns:
            dict: response containing keys including 'data', containing the open interest information
        """
        call_data = {}
        put_data = {}

        options_chain = self.get_options_chain(expiration_date=expiration_date).get('data')
        for strike in options_chain:
            if strike['option_type'] == 'call':
                call_data[strike['strike']] = strike['volume']
            elif strike['option_type'] == 'put':
                put_data[strike['strike']] = strike['volume']

        call_list = [{'strike': strike, 'volume': volume} for strike, volume in call_data.items()]
        put_list = [{'strike': strike, 'volume': volume} for strike, volume in put_data.items()]

        return {
            'description': 'volume',
            'root_symbol': self.symbol,
            'expiration_date': expiration_date,
            'data': {
                "Calls": call_list,
                "Puts": put_list
            }
        }

    def plot_volume(self, expiration_date: str) -> str:
        """
        Plot volume chart for calls and puts; 2 traces (bars)
        """
        volume = self._get_volume(expiration_date=expiration_date)
        calls = volume.get('data', {}).get('Calls', [])
        puts = volume.get('data', {}).get('Puts', [])

        # convert lists to DataFrames
        df_calls = pd.DataFrame(calls)
        df_puts = pd.DataFrame(puts)

        trace_calls = go.Bar(
            x=df_calls['strike'],
            y=df_calls['volume'],
            name='Calls',
            marker={
                'color': 'green'
            }
        )
        trace_puts = go.Bar(
            x=df_puts['strike'],
            y=df_puts['volume'],
            name='Puts',
            marker={
                'color': 'red'
            }
        )

        fig = go.Figure(data=[trace_calls, trace_puts])

        # update layout for stacked bar mode
        fig.update_layout(
            barmode='stack',
            title='Volume',
            xaxis_title='Strike',
            yaxis_title='Volume',
            height=600,
        )

        plot_html = pio.to_html(fig, full_html=False)
        return plot_html

    def _get_implied_volatility(self, expiration_date: str) -> dict:
        """
        Get the implied volatility measurement of each strike for a specific expiration date

        Parameters:
             expiration_date (str): expiration date of format 'YYYY-MM-DD'

        Returns:
            dict: response containing keys including 'data', containing the open interest information:
        """
        try:
            call_data = {}
            put_data = {}

            options_chain = self.get_options_chain(expiration_date=expiration_date).get('data')
            for strike in options_chain:
                if strike['option_type'] == 'call':
                    call_data[strike['strike']] = strike['greeks'].get('mid_iv')
                elif strike['option_type'] == 'put':
                    put_data[strike['strike']] = strike['greeks'].get('mid_iv')

            call_list = [{'strike': strike, 'iv': iv} for strike, iv in call_data.items()]
            put_list = [{'strike': strike, 'iv': iv} for strike, iv in put_data.items()]

            return {
                'description': 'implied volatility',
                'root_symbol': self.symbol,
                'expiration_date': expiration_date,
                'data': {
                    "Calls": call_list,
                    "Puts": put_list
                }
            }
        except:
            return {}

    def plot_iv(self, expiration_date: str) -> str:
        """
        Plot implied volatility chart for calls and puts; 2 traces
        """
        try:
            volume = self._get_implied_volatility(expiration_date=expiration_date)
            calls = volume.get('data', {}).get('Calls', [])
            puts = volume.get('data', {}).get('Puts', [])

            # convert lists to DataFrames
            df_calls = pd.DataFrame(calls)
            df_puts = pd.DataFrame(puts)

            trace_calls = go.Scatter(
                x=df_calls['strike'],
                y=df_calls['iv'],
                mode='lines+markers',
                name='Calls',
                line=dict(color='green')
            )
            trace_puts = go.Scatter(
                x=df_puts['strike'],
                y=df_puts['iv'],
                mode='lines+markers',
                name='Puts',
                line=dict(color='red')
            )

            fig = go.Figure(data=[trace_calls, trace_puts])

            # update layout
            fig.update_layout(
                title='Implied Volatility (multiply by 100%)',
                xaxis_title='Strike',
                yaxis_title='Implied Volatility',
                height=600,
            )

            plot_html = pio.to_html(fig, full_html=False)
            return plot_html
        except:
            return {}

    def _get_last_bid_ask(self, expiration_date: str) -> dict:
        """
        Get the last, bid, ask prices of each strike for a specific expiration date

        Parameters:
             expiration_date (str): expiration date of format 'YYYY-MM-DD'

        Returns:
            dict: response containing keys including 'data', containing the open interest information:
        """
        call_data = {}
        put_data = {}

        options_chain = self.get_options_chain(expiration_date=expiration_date).get('data')
        for strike in options_chain:
            if strike['option_type'] == 'call':
                call_data[strike['strike']] = {'last': strike['last'], 'bid': strike['bid'], 'ask': strike['ask']}
            elif strike['option_type'] == 'put':
                put_data[strike['strike']] = {'last': strike['last'], 'bid': strike['bid'], 'ask': strike['ask']}

        call_list = [{'strike': strike, 'last_bid_ask': last_bid_ask} for strike, last_bid_ask in call_data.items()]
        put_list = [{'strike': strike, 'last_bid_ask': last_bid_ask} for strike, last_bid_ask in put_data.items()]

        return {
            'description': 'last, bid, ask prices',
            'root_symbol': self.symbol,
            'expiration_date': expiration_date,
            'data': {
                "Calls": call_list,
                "Puts": put_list
            }
        }

    def plot_last_bid_ask(self, expiration_date: str) -> str:
        """
        Plot last/bid/ask prices chart for calls and puts; 6 traces total
        """
        last_bid_ask = self._get_last_bid_ask(expiration_date=expiration_date)
        calls = last_bid_ask.get('data', {}).get('Calls', [])
        puts = last_bid_ask.get('data', {}).get('Puts', [])

        # for each strike in both calls and puts there is a last, bid, ask price
        calls_strike = [strike.get('strike') for strike in calls]
        calls_last = [strike.get('last_bid_ask').get('last') for strike in calls]
        calls_bid = [strike.get('last_bid_ask').get('bid') for strike in calls]
        calls_ask = [strike.get('last_bid_ask').get('ask') for strike in calls]

        puts_strike = [strike.get('strike') for strike in puts]
        puts_last = [strike.get('last_bid_ask').get('last') for strike in puts]
        puts_bid = [strike.get('last_bid_ask').get('bid') for strike in puts]
        puts_ask = [strike.get('last_bid_ask').get('ask') for strike in puts]

        # # make subplot of two rows to plot two graphs sharing an x-axis, the timestamps.
        fig = subplots.make_subplots(
            rows=2,
            cols=1,
            shared_xaxes=False,
            vertical_spacing=0.1,
            row_heights=[0.5, 0.5]
        )

        # calls
        fig.add_trace(
            go.Scatter(
                x=calls_strike,
                y=calls_last,
                mode='lines+markers',
                name='Calls Last',
                line=dict(color='green')
            ),
            row=1,
            col=1
        )
        fig.add_trace(
            go.Scatter(
                x=calls_strike,
                y=calls_bid,
                mode='lines+markers',
                name='Calls Bid',
                line=dict(color='purple')
            ),
            row=1,
            col=1
        )
        fig.add_trace(
            go.Scatter(
                x=calls_strike,
                y=calls_ask,
                mode='lines+markers',
                name='Calls Ask',
                line=dict(color='blue')
            ),
            row=1,
            col=1
        )

        # puts
        fig.add_trace(
            go.Scatter(
                x=puts_strike,
                y=puts_last,
                mode='lines+markers',
                name='Puts Last',
                line=dict(color='red')
            ),
            row=2,
            col=1
        )
        fig.add_trace(
            go.Scatter(
                x=puts_strike,
                y=puts_bid,
                mode='lines+markers',
                name='Puts Bid',
                line=dict(color='orange')
            ),
            row=2,
            col=1
        )
        fig.add_trace(
            go.Scatter(
                x=puts_strike,
                y=puts_ask,
                mode='lines+markers',
                name='Puts Ask',
                line=dict(color='yellow')
            ),
            row=2,
            col=1
        )

        fig.update_layout(
            title='Last, Bid, Ask Prices',
            yaxis1_title='Price (Calls)',
            yaxis2_title='Price (Puts)',
            xaxis1_title='Strike',
            xaxis2_title='Strike',
            height=800
        )

        plot_html = pio.to_html(fig, full_html=False)

        return plot_html

    def _get_greeks(self, expiration_date: str) -> dict:
        """
        Get the desired greeks data for a specific expiration date for both calls and puts

        Parameters:
            expiration_date (str): expiration date of format 'YYYY-MM-DD'

        Returns:
            dict: response containing keys including 'data', which contains the specific greeks data
        """
        call_greeks = []
        put_greeks = []

        option_chain = self.get_options_chain(expiration_date=expiration_date).get('data')
        for strike in option_chain:
            if strike['option_type'] == 'call':
                call_greeks.append({'strike': strike['strike'], 'greeks': strike.get('greeks')})
            elif strike['option_type'] == 'put':
                put_greeks.append({'strike': strike['strike'], 'greeks': strike.get('greeks')})

        return {
            'description': 'greeks',
            'root_symbol': self.symbol,
            'expiration_date': expiration_date,
            'data': {
                "Calls": call_greeks,
                "Puts": put_greeks
            }
        }

    def plot_greeks(self, expiration_date: str, greek_letter: str) -> str:
        """
        Plot the greeks data for a specific expiration date for a specific greek letter
        """
        greeks_ = self._get_greeks(expiration_date=expiration_date).get('data')
        calls = {}
        puts = {}

        for strike in greeks_.get('Calls'):
            calls[strike['strike']] = strike['greeks'].get(greek_letter)
        for strike in greeks_.get('Puts'):
            puts[strike['strike']] = strike['greeks'].get(greek_letter)

        # Create data for plotting
        call_strikes = list(calls.keys())
        call_values = list(calls.values())
        put_strikes = list(puts.keys())
        put_values = list(puts.values())

        fig = go.Figure()

        # calls line
        fig.add_trace(
            go.Scatter(
                x=call_strikes,
                y=call_values,
                mode='lines+markers',
                name='Calls',
                line=dict(color='green'),
                marker=dict(size=5),
                text=[f'Strike={strike}, {greek_letter}={value}' for strike, value in zip(call_strikes, call_values)],
                hovertemplate='%{text}<extra></extra>'
            )
        )

        # puts line
        fig.add_trace(
            go.Scatter(
                x=put_strikes,
                y=put_values,
                mode='lines+markers',
                name='Puts',
                line=dict(color='red'),
                marker=dict(size=5),
                text=[f'Strike={strike}, {greek_letter}={value}' for strike, value in zip(put_strikes, put_values)],
                hovertemplate='%{text}<extra></extra>'
            )
        )

        fig.update_layout(
            title=f'{greek_letter}',
            xaxis_title='Strike Price',
            yaxis_title=f'{greek_letter} Value'
        )

        plot_html = pio.to_html(fig, full_html=False)
        return plot_html
