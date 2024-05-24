import yfinance as yf
import warnings
import pandas as pd

warnings.filterwarnings("ignore", category=FutureWarning)


class YFinance:
    """
    Class containing methods for fetching data from the yfinance API.
    """
    def __init__(self, symbol):
        self.symbol = symbol
        self.ticker = yf.Ticker(symbol)

    def get_ohlcv(self) -> pd.DataFrame:
        """
        Get OHLCV data for the symbol for same day.

        Returns:
            DataFrame: OHLCV data for the period '1d'.
        """
        try:
            ohlcv = self.ticker.history(period='1d')
            return ohlcv
        except Exception as e:
            print(f"Error fetching OHLCV data for {self.symbol}: {e}")

    def get_shares_outstanding(self) -> int:
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

    def get_underlying_for_main_info(self) -> dict:
        """
        This method is used to get the market and pre-/post-market prices/price changes for a ticker,
        as well as bid/ask and sizes, among other things.

        Returns:
            dict: Dictionary of miscellaneous underlying information for the symbol with the following
            keys: ['language', 'region', 'quoteType', 'typeDisp', ...] (length of 81)
        """
        try:
            underlying = self.ticker.option_chain().underlying
            return underlying
        except Exception as e:
            print(f"Error fetching underlying info for {self.symbol}: {e}")

    def get_fast_info(self) -> "Fast Info":
        """
        Get miscellaneous quick access info about a ticker such as company market capitalization.

        Returns:
            FastInfo: An instance containing fast info metrics like fiftyDayAverage and marketCap
        """
        try:
            fast_info = self.ticker.get_fast_info()
            return fast_info
        except Exception as e:
            print(f"Error fetching fast info for {self.symbol}: {e}")

    def get_calendar(self) -> dict:
        """
        Get the calendar containing dividend dates, EPS estimates, and revenue estimates for upcoming quarter.

        Returns:
            dict: 2 more keys for companies that pay dividends
        """
        try:
            div_eps_rev_calendar = self.ticker.get_calendar()
            return div_eps_rev_calendar
        except Exception as e:
            print(f"Error fetching calendar info for {self.symbol}: {e}")

    def get_institutional_holders(self) -> any or list[dict]:
        """
        Get the institutional holders for a ticker.

        Returns:
            list: List of dictionaries, containing keys 'Date Reported', 'Holder', 'pctHeld', 'Shares', 'Value'
        """
        try:
            institutions = self.ticker.get_institutional_holders().to_dict(orient='records')
            if institutions == '[]':
                return None
            return institutions
        except Exception as e:
            print(f"Error fetching institutional holders for {self.symbol}: {e}")

    def get_insider_transactions(self) -> any or list[dict]:
        """
        Get the insider transactions data for the symbol.

        Returns:
            list: List of dictionaries, containing keys about insider purchase, sell, or option exercise
        """
        try:
            insider_transactions = self.ticker.get_insider_transactions().to_dict(orient='records')
            if insider_transactions == '[]':
                return None
            return insider_transactions
        except Exception as e:
            print(f"Error fetching insider transactions for {self.symbol}: {e}")

    def get_analyst_ratings(self) -> dict:
        """
        Get the analyst ratings for a ticker.
        Because there seem to be repeated firm ratings, we get the most recent ones (latest occurrence) only

        Returns:
            dict: Dictionary of analyst ratings, {firm_name: {dict containing 'GradeDate', 'Action', etc.}}
        """
        # index of original df is 'GradeDate' which we want to include
        analyst_ratings_df = self.ticker.get_upgrades_downgrades().reset_index()
        analyst_ratings_dict = analyst_ratings_df.to_dict('records')

        ratings_by_unique_firms = {}

        for rating in analyst_ratings_dict:
            firm_name = rating['Firm']
            if firm_name not in ratings_by_unique_firms:
                # If the firm is not already seen, add it to the unique_ratings dictionary
                ratings_by_unique_firms[firm_name] = rating

        return ratings_by_unique_firms

    def get_expiry_list(self) -> tuple:
        """
        Get the expiry list for the ticker.
        Each element of the tuple is a string representing an expiry date formatted as YYYY-MM-DD.

        Returns:
            tuple: Tuple of expiry dates of all option chains for a given ticker symbol.
        """
        expiry_dates = self.ticker.options
        return expiry_dates

    def get_option_chain_for_expiry(self, expiry_date: str) -> object:
        """
        Get the yfinance option chain (calls and puts) for a given ticker symbol and expiry date.

        Parameters:
            expiry_date (string): Expiry date for a given ticker symbol, formatted as YYYY-MM-DD.

        Returns:
            object: yfinance option chain for an expiry date.
        """
        option_chain = self.ticker.option_chain(date=expiry_date)
        return option_chain

    def _get_open_interest(self, expiry_date: str) -> list[dict[float, int]]:
        """
        Given an arg expiry date of format 'YYYY-MM-DD', get the call and put open interest of all option chains.
        We utilize the get_option_chain_for_expiry() method to get the option chain

        Parameters:
            expiry_date (string): Expiry date for a given ticker symbol, formatted as YYYY-MM-DD.

        Returns:
            list: List of length 2, dictionaries containing key-value pairs of strike prices and open interest for each
        """
        # get the option chain for the specified expiry date
        option_chain = self.get_option_chain_for_expiry(expiry_date)

        # extract the call and put data from the option chain
        calls = option_chain.calls
        puts = option_chain.puts

        # initialize dictionaries
        call_open_interest = {}
        put_open_interest = {}

        # populate dictionaries with strike prices and corresponding openInterest
        for _, row in calls.iterrows():
            call_open_interest[row['strike']] = row['openInterest']

        for _, row in puts.iterrows():
            put_open_interest[row['strike']] = row['openInterest']

        # return list
        return [call_open_interest, put_open_interest]

    def _get_volume(self, expiry_date: str) -> list[dict[float, int]]:
        """
        Given an arg expiry date of format 'YYYY-MM-DD', get the call and put volume of all option chains.
        We utilize the get_option_chain_for_expiry() method to get the option chain

        Parameters:
            expiry_date (string): Expiry date for a given ticker symbol, formatted as YYYY-MM-DD.

        Returns:
            list: List of length 2, dictionaries containing key-value pairs of strike prices and volume for each
        """
        option_chain = self.get_option_chain_for_expiry(expiry_date)

        calls = option_chain.calls
        puts = option_chain.puts

        call_volume = {}
        put_volume = {}

        for _, row in calls.iterrows():
            call_volume[row['strike']] = row['volume']

        for _, row in puts.iterrows():
            put_volume[row['strike']] = row['volume']

        return [call_volume, put_volume]

    def _get_implied_volatility(self, expiry_date: str) -> list[dict[float, int]]:
        """
        Given an arg expiry date of format 'YYYY-MM-DD', get the call and put implied volatility of all option chains.
        We utilize the get_option_chain_for_expiry() method to get the option chain

        Parameters:
            expiry_date (string): Expiry date for a given ticker symbol, formatted as YYYY-MM-DD.

        Returns:
            list: List of length 2, dictionaries containing key-value pairs of strike prices and
            implied volatility for each
        """
        option_chain = self.get_option_chain_for_expiry(expiry_date)

        calls = option_chain.calls
        puts = option_chain.puts

        call_iv = {}
        put_iv = {}

        for _, row in calls.iterrows():
            call_iv[row['strike']] = row['impliedVolatility']

        for _, row in puts.iterrows():
            put_iv[row['strike']] = row['impliedVolatility']

        return [call_iv, put_iv]

    def _get_last_price_bid_ask(self, expiry_date: str) -> list[dict[float, list[float]]]:
        """
        Given an arg expiry date of format 'YYYY-MM-DD', get the last price, bid price, ask price
        for calls and puts of all option chains.
        We utilize the get_option_chain_for_expiry() method to get the option chain

        Parameters:
            expiry_date (string): Expiry date for a given ticker symbol, formatted as YYYY-MM-DD.

        Returns:
            list: List of length 2, dictionaries containing key-value pairs of strike prices and a list for each
            containing the last price, bid, ask for calls and puts.
        """
        option_chain = self.get_option_chain_for_expiry(expiry_date)

        calls = option_chain.calls
        puts = option_chain.puts

        call_lpba = {}
        put_lpba = {}

        for _, row in calls.iterrows():
            call_lpba[row['strike']] = [row['lastPrice'], row['bid'], row['ask']]

        for _, row in puts.iterrows():
            put_lpba[row['strike']] = [row['lastPrice'], row['bid'], row['ask']]

        return [call_lpba, put_lpba]
