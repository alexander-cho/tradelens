import yfinance as yf


def get_underlying():
    ticker = yf.Ticker('SOFI')
    return ticker.option_chain().underlying


def get_call_options():
    ticker = yf.Ticker('SOFI')
    return ticker.option_chain().calls


def get_put_options():
    ticker = yf.Ticker('SOFI')
    return ticker.option_chain().puts


def get_open_interest():
    pass


def get_last_price():
    pass


def get_volume():
    pass
