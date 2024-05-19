import yfinance as yf
import warnings

warnings.filterwarnings("ignore", category=FutureWarning)


class YFinance:
    def __init__(self, symbol):
        self.symbol = symbol
        self.ticker = yf.Ticker(symbol)

    def get_ohlcv(self):
        """
        Get OHLCV data for the symbol for same day.

        Returns:
            DataFrame: OHLCV data for the period '1d'.
        """
        return self.ticker.history(period='1d')

    def get_shares_outstanding(self):
        """
        Get the number of shares outstanding for the symbol.

        Returns:
            int: Number of shares outstanding.
        """
        try:
            outstanding_shares = self.ticker.info['sharesOutstanding']
            return outstanding_shares
        except Exception as e:
            print(f"Error fetching implied shares outstanding for {self.symbol}: {e}")

    def get_underlying_for_main_info(self):
        """
        This method is used to get the market and pre-/post-market prices/price changes for a ticker,
        as well as bid/ask and sizes, among other things.

        Returns:
            dict: Dictionary of miscellaneous underlying information for the symbol.
            keys: ['language', 'region', 'quoteType', 'typeDisp', ...] (length of 81)
        """
        try:
            underlying = self.ticker.option_chain().underlying
            return underlying
        except Exception as e:
            print(f"Error fetching underlying info for {self.symbol}: {e}")
