import yfinance as yf


def get_expiry_list(symbol):
    ticker = yf.Ticker(symbol)
    return ticker.options


def get_options_detail(symbol):
    ticker = yf.Ticker(symbol)
    expiry_dates = get_expiry_list(symbol)
    options_data = {}
    # populate dictionary with {"expiry date": option chain for that expiry}
    for expiry in expiry_dates:
        options_data[expiry] = ticker.option_chain(date=expiry)
    return options_data
