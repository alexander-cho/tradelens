import yfinance as yf


def get_expiry_list(symbol) -> tuple:
    """
    This method returns a tuple of expiry dates of all option chains for a given ticker symbol.
    Each element of the tuple is a string representing an expiry date formatted as YYYY-MM-DD.
    """
    ticker = yf.Ticker(symbol)
    return ticker.options


def get_option_chain_for_expiry(symbol, expiry_date) -> object:
    """
    This method returns a yfinance option chain (calls and puts) for a given ticker symbol and expiry date.
    """
    ticker = yf.Ticker(symbol)
    option_chain = ticker.option_chain(date=expiry_date)
    return option_chain
