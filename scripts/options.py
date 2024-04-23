import yfinance as yf


ticker = yf.Ticker('SOFI')
options = (ticker.option_chain(ticker.options[5]))  # index 0 in this case: next expiry date
print(options.calls.iloc[10])


def get_call_options():
    ticker = yf.Ticker('SOFI')
    options = (ticker.option_chain(ticker.options[0]))
    return options.calls['strike'].to_list()


def get_put_options():
    pass


def get_open_interest():
    pass


def get_last_price():
    pass


def get_volume():
    pass



