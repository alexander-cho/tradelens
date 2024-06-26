from modules.providers.yfinance_ import YFinance
from modules.providers.tradier import Tradier


class MaxPain:
    def __init__(self, symbol):
        self.yfinance = YFinance(symbol)
        self.tradier = Tradier(symbol)

    def get_current_price(self):
        current_price = self.yfinance.get_underlying_for_price_info().get('regularMarketPrice')
        return current_price

    def calculate_cash_values(self, expiration_date):
        options_chain = self.tradier.get_options_chain(expiration_date)

        call_cash_values = []
        put_cash_values = []

        for strike in options_chain:
            if 'Call' in strike.get('description'):
                cash_value = strike.get('open_interest') * (float(strike.get('strike')) - self.get_current_price())
                call_cash_values.append(cash_value)
            else:
                cash_value = strike.get('open_interest') * (self.get_current_price() - float(strike.get('strike')))
                put_cash_values.append(cash_value)

        return {
            'Calls': call_cash_values,
            'Puts': put_cash_values
        }
