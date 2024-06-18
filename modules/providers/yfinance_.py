import yfinance as yf
import warnings

warnings.filterwarnings("ignore", category=FutureWarning)


class YFinance:
    """
    Class containing methods for fetching data from the yfinance API.
    """
    def __init__(self, symbol: str):
        self.symbol = symbol
        self.ticker = yf.Ticker(symbol)

    def get_basic_info(self) -> dict:
        """
        Get the basic information for the symbol, such as number of shares outstanding and short interest

        Returns:
            dict: Basic information for the symbol.
        """
        try:
            basic_info = self.ticker.info
            return basic_info
        except Exception as e:
            print(f"Error fetching implied shares outstanding for {self.symbol}: {e}")

    def get_underlying_for_price_info(self) -> dict:
        """
        This method is used to get the market and pre-/post-market prices/price changes for a ticker,
        as well as bid/ask and sizes, among other things.

        Returns:
            dict: Dictionary of miscellaneous underlying information for the symbol with the following
            keys: keys to keep as defined in list form below
        """
        try:
            underlying_info = self.ticker.option_chain().underlying

            # new dict for keeping keys we want from the response
            price_info = {}
            keys_to_keep = ['regularMarketPrice',
                            'regularMarketChange',
                            'regularMarketChangePercent',
                            'postMarketPrice',
                            'postMarketChange',
                            'postMarketChangePercent',
                            'bid',
                            'ask',
                            'bidSize',
                            'askSize']

            for key in keys_to_keep:
                if key in underlying_info:
                    price_info[key] = underlying_info[key]
            return price_info
        except Exception as e:
            print(f"Error fetching underlying info for {self.symbol}: {e}")
            return {}

    def get_fast_info(self) -> dict:
        """
        Get miscellaneous quick access info about a ticker such as company market capitalization.

        Returns:
            FastInfo: An instance containing fast info metrics like fiftyDayAverage and marketCap
        """
        try:
            fast_info = self.ticker.get_fast_info()
            fast_info_as_dict = {}

            for key in fast_info.keys():
                fast_info_as_dict[key] = fast_info[key]
            return fast_info_as_dict
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

    def get_balance_sheet(self) -> dict:
        """

        """
        pass

    def get_cashflow(self) -> dict:
        """

        """
        pass

    def get_income_statement(self) -> dict:
        """

        """
        pass

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
        try:
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
        except Exception as e:
            print(f"Error fetching analyst ratings for {self.symbol}: {e}")

    def get_expiry_list(self) -> tuple:
        """
        Get the expiry list for the ticker.
        Each element of the tuple is a string representing an expiry date formatted as YYYY-MM-DD.

        Returns:
            tuple: Tuple of expiry dates of all option chains for a given ticker symbol.
        """
        try:
            expiry_dates = self.ticker.options
            return expiry_dates
        except Exception as e:
            print(f"Error fetching expiry list for {self.symbol}: {e}")

    def _get_option_chain_for_expiry(self, expiry_date: str) -> object:
        """
        Get the yfinance option chain (calls and puts) for a given ticker symbol and expiry date.

        Parameters:
            expiry_date (str): Expiry date for a given ticker symbol, formatted as YYYY-MM-DD.

        Returns:
            object: yfinance option chain for an expiry date.
        """
        try:
            option_chain = self.ticker.option_chain(date=expiry_date)
            return option_chain
        except Exception as e:
            print(f"Error fetching option chain for {self.symbol}: {e}")

    def _extract_options_data(self, expiry_date: str, attribute: str) -> dict[str, dict]:
        """
        Extract specific attributes from the option chain.
        The option chain is of type _namedTuple Options, according to the yfinance implementation, that contains the
        attributes calls, puts, underlying, the first two being of type pd.DataFrame

        Parameters:
            expiry_date (str): Option expiry date of format 'YYYY-MM-DD'
            attribute (str): Attribute to extract from option chain
                e.g. 'openInterest', 'volume', 'impliedVolatility', 'lastPrice', 'bid', 'ask'

        Returns:
            dict: Dictionary containing Call and Put data of strike price and the specified attribute for each.
        """
        # to populate with corresponding data
        call_data = {}
        put_data = {}

        try:
            option_chain = self._get_option_chain_for_expiry(expiry_date)

            # split the chain between calls and puts
            calls = option_chain.calls
            puts = option_chain.puts

            for _, row in calls.iterrows():
                if attribute != 'lpba':
                    call_data[row['strike']] = row[attribute]
                else:
                    call_data[row['strike']] = [row['lastPrice'], row['bid'], row['ask']]

            for _, row in puts.iterrows():
                if attribute != 'lpba':
                    put_data[row['strike']] = row[attribute]
                else:
                    put_data[row['strike']] = [row['lastPrice'], row['bid'], row['ask']]

            response = {"symbol": self.symbol, "expiry_date": expiry_date, "data": {"Calls": call_data, "Puts": put_data}}
            return response
        except Exception as e:
            print(f"Error extracting options data for {self.symbol}: {e}")

    def _get_open_interest(self, expiry_date: str) -> dict:
        """
        Get the open interest data for call and put options for a given expiry date.

        Parameters:
            expiry_date (str): Expiry date for a given ticker symbol, formatted as YYYY-MM-DD.

        Returns:
            dict: Dictionary containing Call and Put data of strike price and open interest for each.
        """
        open_interest = self._extract_options_data(expiry_date, attribute='openInterest')
        return open_interest

    def _get_volume(self, expiry_date: str) -> dict:
        """
        Get the volume data for call and put options for a given expiry date.

        Parameters:
            expiry_date (str): Expiry date for a given ticker symbol, formatted as YYYY-MM-DD.

        Returns:
            dict: Dictionary containing Call and Put data of strike price and volume for each.
        """
        volume = self._extract_options_data(expiry_date, attribute='volume')
        return volume

    def _get_implied_volatility(self, expiry_date: str) -> dict:
        """
        Get the implied volatility data for call and put options for a given expiry date.

        Parameters:
            expiry_date (str): Expiry date for a given ticker symbol, formatted as YYYY-MM-DD.

        Returns:
            dict: Dictionary containing Call and Put data of strike price and implied volatility for each.
        """
        implied_volatility = self._extract_options_data(expiry_date, attribute='impliedVolatility')
        return implied_volatility

    def _get_last_price_bid_ask(self, expiry_date: str) -> dict:
        """
        Get the last price, bid, ask data for call and put options for a given expiry date.

        Parameters:
            expiry_date (str): Expiry date for a given ticker symbol, formatted as YYYY-MM-DD.

        Returns:
            dict: Dictionary containing Call and Put data of strike price and open interest for each.
        """
        lpba = self._extract_options_data(expiry_date, attribute='lpba')
        return lpba

    def draw_open_interest_chart(self, expiry_date: str):
        call_oi = self._get_open_interest(expiry_date).get('data').get('Calls')
        put_oi = self._get_open_interest(expiry_date).get('data').get('Puts')
        return call_oi, put_oi
