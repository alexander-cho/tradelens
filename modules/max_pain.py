import pandas as pd

import plotly.graph_objects as go
import plotly.io as pio

from modules.providers.tradier import Tradier


class MaxPain:
    """
    Get call/put/sum cash values and max pain using the open interest data from the Tradier class option chain
    """
    def __init__(self, symbol: str):
        self.symbol = symbol
        self.tradier = Tradier(symbol)

    def get_cash_values(self, expiration_date: str) -> dict:
        """
        Get the notional cash amount for the calls and puts in an options chain.
        Using these, sum up the two and find the lowest value to obtain the maximum pain strike price.

        Parameters:
            expiration_date (str): The options expiration date in YYYY-MM-DD format.

        Returns:
            dict: data containing cash values for each strike for calls, puts, the sum of both, and max pain.
                The first three are lists of dictionaries containing k, v of strike and combined cash value
                for each strike.
                The max pain is of a dictionary containing 2 key value pairs of strike and combined cash value.
        """
        # get the open interest data for each strike price
        open_interest_for_chain = self.tradier.get_open_interest(expiration_date=expiration_date)

        # handle case where data is missing or empty
        if not open_interest_for_chain or 'data' not in open_interest_for_chain:
            return []

        # within the open interest data get the calls and puts
        calls_data = open_interest_for_chain['data'].get('Calls', [])
        puts_data = open_interest_for_chain['data'].get('Puts', [])

        # extract the strike prices
        call_strikes = [call['strike'] for call in calls_data]
        put_strikes = [put['strike'] for put in puts_data]

        # extract the open interest
        call_open_interest = [call['open_interest'] for call in calls_data]
        put_open_interest = [put['open_interest'] for put in puts_data]

        # to see the cash loss value, hypothetical underlying closing prices will be made out of each strike price
        hypothetical_call_closes = sorted(set(call_strikes))
        hypothetical_put_closes = sorted(set(put_strikes))

        # initialize list for cash values at each strike
        call_cash_values = []
        put_cash_values = []

        for close in hypothetical_call_closes:
            # initialize a sum to 0, so we can collect the cash values for each strike at each close
            call_cash_sum = 0

            # for each of those strikes
            for i in range(len(call_strikes)):
                # get the strike and open interest of the current iteration
                strike = call_strikes[i]
                open_interest = call_open_interest[i]

                # if the cash value is negative, in other words the call is in the money, the cash value is set to 0
                if (close - strike) * open_interest * 100 < 0:
                    call_cash_value = 0
                else:
                    # assign the cash value at that strike by the following equation
                    call_cash_value = (close - strike) * open_interest * 100

                # add the cash value at that strike to the sum
                call_cash_sum += call_cash_value

            # after the sum is calculated for the whole hypothetical close, add it to the list
            call_cash_values.append({'strike': close, 'cash': call_cash_sum})

        # Puts follow similar logic, but it's the strike price minus close
        for close in hypothetical_put_closes:
            put_cash_sum = 0

            for i in range(len(put_strikes)):
                strike = put_strikes[i]
                open_interest = put_open_interest[i]
                if (strike - close) * open_interest * 100 < 0:
                    put_cash_value = 0
                else:
                    put_cash_value = (strike - close) * open_interest * 100

                put_cash_sum += put_cash_value

            put_cash_values.append({'strike': close, 'cash': put_cash_sum})

        # get the sum of the call and put cash values for each strike
        sum_cash_values = []
        for i in range(len(call_cash_values)):
            sum_ = call_cash_values[i].get('cash') + put_cash_values[i].get('cash')
            sum_cash_values.append({'strike': call_cash_values[i].get('strike'), 'cash': sum_})

        # get the strike where max pain occurs
        max_pain_cash_value = min([i.get('cash') for i in sum_cash_values])
        max_pain_strike = next(i.get('strike') for i in sum_cash_values if i.get('cash') == max_pain_cash_value)

        max_pain = {'strike': max_pain_strike, 'cash': max_pain_cash_value}

        data = {
            'Calls': call_cash_values,
            'Puts': put_cash_values,
            'Sums': sum_cash_values,
            'max_pain': max_pain
        }

        return {
            'ticker': self.symbol,
            'expiration_date': expiration_date,
            'data': data
        }

    def plot_cash_values(self, expiration_date: str) -> str:
        """
        Plot cash values and max pain as a stacked bar chart with calls and puts sharing a similar x-axis of the strike
        """
        cash_values = self.get_cash_values(expiration_date=expiration_date)
        calls = cash_values.get('data', {}).get('Calls', [])
        puts = cash_values.get('data', {}).get('Puts', [])
        max_pain = cash_values.get('data', {}).get('max_pain', {}).get('strike', 0)

        # convert lists to DataFrames
        df_calls = pd.DataFrame(calls)
        df_puts = pd.DataFrame(puts)

        trace_calls = go.Bar(
            x=df_calls['strike'],
            y=df_calls['cash'],
            name='Calls',
            marker={
                'color': 'green'
            }
        )
        trace_puts = go.Bar(
            x=df_puts['strike'],
            y=df_puts['cash'],
            name='Puts',
            marker={
                'color': 'red'
            }
        )

        fig = go.Figure(data=[trace_calls, trace_puts])

        # update layout for stacked bar mode
        fig.update_layout(
            barmode='stack',
            title=f'Cash | Max Pain lies at {max_pain}',
            xaxis_title='Strike Price',
            yaxis_title='Cash (USD)',
            height=500
        )

        plot_html = pio.to_html(fig, full_html=False)
        return plot_html
