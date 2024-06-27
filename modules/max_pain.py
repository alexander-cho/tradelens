from modules.providers.tradier import Tradier


class MaxPain:
    def __init__(self, symbol):
        self.tradier = Tradier(symbol)

    def calculate_cash_values(self, expiration_date):
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
            sum_cash_values.append({'strike': i, 'cash': sum_})

        calls_puts_sums = {
            'calls': call_cash_values,
            'puts': put_cash_values,
            'sums': sum_cash_values
        }

        return {
            'expiration_date': expiration_date,
            'data': calls_puts_sums
        }
