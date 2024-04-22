import yfinance as yf


ticker = yf.Ticker('SOFI')
options = (ticker.option_chain(ticker.options[0]))  # index 0 in this case: next expiry date
print(options.calls)


def get_call_options():
    ticker = yf.Ticker('SOFI')
    options = (ticker.option_chain(ticker.options[0]))
    return options.calls
