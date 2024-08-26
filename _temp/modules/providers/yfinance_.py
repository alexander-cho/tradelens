import warnings
import pandas as pd

import yfinance as yf

import plotly.graph_objects as go
import plotly.io as pio


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
            keys_to_keep = [
                'address1', 'city', 'state', 'country', 'zip',
                'industry', 'sector', 'fullTimeEmployees', 'companyOfficers',
                'forwardPE', 'forwardEps', 'fiftyDayAverage', 'twoHundredDayAverage',
                'floatShares', 'sharesOutstanding', 'sharesShort', 'longBusinessSummary',
                'shortPercentOfFloat', 'heldPercentInsiders', 'heldPercentInstitutions',
                'targetHighPrice', 'targetLowPrice', 'targetMeanPrice', 'targetMedianPrice',
                'numberOfAnalystOpinions', 'marketCap', 'fiftyTwoWeekHigh', 'fiftyTwoWeekLow'
            ]

            # new dictionary with keys to keep and values corresponding to it
            filtered_info = {key: basic_info.get(key) for key in keys_to_keep}
            return {
                'description': 'ticker basic information',
                'symbol': self.symbol,
                'data': filtered_info
            }
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
            keys_to_keep = [
                'regularMarketPrice',
                'regularMarketChange',
                'regularMarketChangePercent',
                'postMarketPrice',
                'postMarketChange',
                'postMarketChangePercent',
                'bid',
                'ask',
                'bidSize',
                'askSize'
            ]

            for key in keys_to_keep:
                if key in underlying_info:
                    price_info[key] = underlying_info[key]
            return {
                'description': 'ticker symbol data about current price, bid, ask',
                'symbol': self.symbol,
                'data': price_info
            }
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
            return {
                'description': 'ticker fast info',
                'symbol': self.symbol,
                'data': fast_info_as_dict
            }
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
            return {
                'description': 'earnings/dividends calendar, earnings/revenue estimates',
                'symbol': self.symbol,
                'data': div_eps_rev_calendar
            }
        except Exception as e:
            print(f"Error fetching calendar info for {self.symbol}: {e}")

    def get_balance_sheet(self) -> dict:
        """
        Get the quarterly balance sheet for a ticker

        Returns:
            dict: keys 'ticker' and 'balance_sheet', latter containing k, v pairs of Timestamp and balance sheet
                attributes
        """
        try:
            balance_sheet = self.ticker.get_balance_sheet(freq='quarterly')

            # transpose df to have timestamps as index and features as columns
            balance_sheet_df = balance_sheet.transpose()

            # store dictionaries for each quarter in new list
            data_list = []

            for date, values in balance_sheet_df.iterrows():
                # rewrite timestamps
                formatted_date = pd.to_datetime(date).strftime('%Y-%m-%d')
                balance_sheet_dict = {
                    'quarter': formatted_date,
                    'balance_sheet': values.to_dict()
                }
                data_list.append(balance_sheet_dict)

            # show the latest quarter at the end
            final_balance_sheet = data_list[::-1]

            return {
                'description': 'quarterly balance sheet',
                'symbol': self.symbol,
                'data': final_balance_sheet
            }
        except Exception as e:
            print(f"Error fetching balance sheet for {self.symbol}: {e}")

    def get_cashflow_statement(self) -> dict:
        """
        Get the quarterly cashflow statement for a ticker

        Returns:
            dict: keys 'ticker' and 'cashflow', latter containing k, v pairs of Timestamp and cashflow attributes
        """
        try:
            cashflow_statement = self.ticker.get_cashflow(freq='quarterly')

            # transpose df to have timestamps as index and features as columns
            cashflow_statement_df = cashflow_statement.transpose()

            # store dictionaries for each quarter in new list
            data_list = []

            for date, values in cashflow_statement_df.iterrows():
                # rewrite timestamps
                formatted_date = pd.to_datetime(date).strftime('%Y-%m-%d')
                cashflow_statement_dict = {
                    'quarter': formatted_date,
                    'cashflow_statement': values.to_dict()
                }
                data_list.append(cashflow_statement_dict)

            # show the latest quarter at the end
            final_cashflow_statement = data_list[::-1]

            return {
                'description': 'quarterly cashflow statement',
                'symbol': self.symbol,
                'data': final_cashflow_statement
            }
        except Exception as e:
            print(f"Error fetching cashflow for {self.symbol}: {e}")

    def get_income_statement(self) -> dict:
        """
        Get the quarterly income statement for a ticker

        Returns:
            dict: keys 'ticker' and 'income_statement', latter containing k, v pairs of Timestamp and income statement
                attributes
        """
        try:
            income_statement = self.ticker.get_incomestmt(freq='quarterly')

            # transpose df to have timestamps as index and features as columns
            income_statement_df = income_statement.transpose()

            # store dictionaries for each quarter in new list
            data_list = []

            for date, values in income_statement_df.iterrows():
                # rewrite timestamps
                formatted_date = pd.to_datetime(date).strftime('%Y-%m-%d')
                income_statement_dict = {
                    'quarter': formatted_date,
                    'income_statement': values.to_dict()
                }
                data_list.append(income_statement_dict)

            # show the latest quarter at the end
            final_income_statement = data_list[::-1]

            return {
                'description': 'quarterly income statement',
                'symbol': self.symbol,
                'data': final_income_statement
            }
        except Exception as e:
            print(f"Error fetching income statement for {self.symbol}: {e}")

    def get_institutional_holders(self) -> dict:
        """
        Get the institutional holders for a ticker.

        Returns:
            list: List of dictionaries, containing keys 'Date Reported', 'Holder', 'pctHeld', 'Shares', 'Value'
        """
        try:
            institutions = self.ticker.get_institutional_holders().to_dict(orient='records')
            if institutions == '[]':
                return None
            return {
                'description': 'top ten institutional holders',
                'ticker': self.symbol,
                'data': institutions
            }
        except Exception as e:
            print(f"Error fetching institutional holders for {self.symbol}: {e}")

    def get_insider_transactions(self) -> dict:
        """
        Get the insider transactions data for the symbol.

        Returns:
            list: List of dictionaries, containing keys about insider purchase, sell, or option exercise
        """
        try:
            insider_transactions = self.ticker.get_insider_transactions().to_dict(orient='records')
            if insider_transactions == '[]':
                return None

            # change key 'Start Date' to 'startDate'
            for transaction in insider_transactions:
                transaction['startDate'] = transaction.pop('Start Date')

            return {
                'description': 'recent insider transactions',
                'ticker': self.symbol,
                'data': insider_transactions
            }
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
                    # ff the firm is not already seen, add it to the unique_ratings dictionary
                    ratings_by_unique_firms[firm_name] = rating

            # convert the dictionary to a list of dictionaries containing only the values, since the keys are firm name
            ratings_list = list(ratings_by_unique_firms.values())

            return {
                'description': 'analyst ratings',
                'ticker': self.symbol,
                'data': ratings_list
            }
        except Exception as e:
            print(f"Error fetching analyst ratings for {self.symbol}: {e}")

    def get_options_expiry_list(self) -> dict:
        """
        Get the options expiry date list for the ticker.
        Each element of the tuple is a string representing an expiry date formatted as YYYY-MM-DD.

        Returns:
            tuple: Tuple of expiry dates of all option chains for a given ticker symbol.
        """
        try:
            expiry_dates = self.ticker.options
            return {
                'description': 'options expiry dates',
                'ticker': self.symbol,
                'data': expiry_dates
            }
        except Exception as e:
            print(f"Error fetching expiry list for {self.symbol}: {e}")

    def plot_institutional_holders(self) -> dict:
        """
        Bar chart for top 10 institutional holders of a stock
        """
        try:
            institutional_holders = self.get_institutional_holders().get('data')
            firm = [holder['Holder'] for holder in institutional_holders]
            num_shares = [holder['Shares'] for holder in institutional_holders]

            # for hover popup
            value = [holder['Value'] for holder in institutional_holders]
            pct_held = [holder['pctHeld'] for holder in institutional_holders]

            trace = go.Bar(
                x=firm,
                y=num_shares,
                marker={'color': 'green'},
                hovertemplate='<b>%{x}</b><br>'
                              'Shares: %{y}<br>'
                              'Percent Held: %{customdata[0]:.2%}<br>'
                              'Value: $%{customdata[1]:,}<extra></extra>',
                customdata=list(zip(pct_held, value))
            )

            fig = go.Figure(data=trace)

            # update layout for bar chart
            fig.update_layout(
                title='Institutional Ownership',
                xaxis_title='Holder',
                yaxis_title='Shares',
                height=700
            )

            plot_html = pio.to_html(fig, full_html=False)
            return {
                'success': True,
                'data': plot_html
            }
        except AttributeError:
            return {
                'success': False,
                'data': 'Institutional ownership not available.'
            }

    def plot_balance_sheet(self) -> str:
        balance_sheet = self.get_balance_sheet().get('data')
        quarter = [quarter['quarter'] for quarter in balance_sheet]
        keys_to_keep = [
            'AccountsPayable',
            'AccountsReceivable',
            'CashAndCashEquivalents',
            'CurrentAssets',
            'LongTermDebt',
            'NetPPE',
            'InvestmentsAndAdvances',
            'NetTangibleAssets',
            'RetainedEarnings',
            'TotalAssets'
        ]

        fig = go.Figure()

        for key in keys_to_keep:
            # create a list of the values of each metric
            y_values = [quarter['balance_sheet'].get(key) for quarter in balance_sheet]
            fig.add_trace(
                go.Scatter(
                    x=quarter,
                    y=y_values,
                    mode='lines',
                    name=key,
                )
            )

        fig.update_layout(
            title='Quarterly Balance Sheet',
            xaxis_title='Date',
            yaxis_title='Value',
            width=1200,
            height=650,
            margin=dict(
                l=10,
                r=10,
                t=30,
                b=10
            )
        )

        # convert plot to HTML string
        plot_html = pio.to_html(fig, full_html=False)

        return plot_html

    def plot_cashflow_statement(self) -> str:
        cashflow_statement = self.get_cashflow_statement().get('data')
        quarter = [quarter['quarter'] for quarter in cashflow_statement]
        keys_to_keep = [
            'CapitalExpenditure',
            'FreeCashFlow',
            'OperatingCashFlow',
            'NetIncomeFromContinuingOperations',
            'DepreciationAndAmortization',
            'ChangeInWorkingCapital',
            'InterestPaidSupplementalData',
            'SaleOfInvestment',
            'CashDividendsPaid',
            'PurchaseOfPPE',
        ]

        fig = go.Figure()

        for key in keys_to_keep:
            # create a list of the values of each metric
            y_values = [quarter['cashflow_statement'].get(key) for quarter in cashflow_statement]
            fig.add_trace(
                go.Scatter(
                    x=quarter,
                    y=y_values,
                    mode='lines',
                    name=key,
                )
            )

        fig.update_layout(
            title='Quarterly Cashflow Statement',
            xaxis_title='Date',
            yaxis_title='Value',
            width=1270,
            height=650,
            margin=dict(
                l=10,
                r=10,
                t=30,
                b=10
            )
        )

        # convert plot to HTML string
        plot_html = pio.to_html(fig, full_html=False)

        return plot_html

    def plot_income_statement(self) -> str:
        income_statement = self.get_income_statement().get('data')
        quarter = [quarter['quarter'] for quarter in income_statement]
        keys_to_keep = [
            'TotalRevenue',
            'GrossProfit',
            'EBIT',
            'EBITDA',
            'OperatingIncome',
            'NetIncome',
            'BasicEPS',
            'DilutedEPS',
            'OperatingExpense',
            'ResearchAndDevelopment'
        ]

        fig = go.Figure()

        for key in keys_to_keep:
            # create a list of the values of each metric
            y_values = [quarter['income_statement'].get(key) for quarter in income_statement]
            fig.add_trace(
                go.Scatter(
                    x=quarter,
                    y=y_values,
                    mode='lines',
                    name=key,
                )
            )

        fig.update_layout(
            title='Quarterly Income Statement',
            xaxis_title='Date',
            yaxis_title='Value',
            width=1200,
            height=650,
            margin=dict(
                l=10,
                r=10,
                t=30,
                b=10
            )
        )

        # convert plot to HTML string
        plot_html = pio.to_html(fig, full_html=False)

        return plot_html
