import yfinance as yf


def get_underlying(symbol):
    ticker = yf.Ticker(symbol)
    return ticker.option_chain().underlying


def get_call_options(symbol):
    ticker = yf.Ticker(symbol)
    return ticker.option_chain().calls


def get_put_options(symbol):
    ticker = yf.Ticker(symbol)
    return ticker.option_chain().puts


def get_open_interest():
    pass


def get_last_price():
    pass


def get_volume():
    pass
