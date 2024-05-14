import yfinance as yf
import warnings

warnings.filterwarnings("ignore", category=FutureWarning)


def get_institutional_holders(symbol):
    ticker = yf.Ticker(symbol)
    try:
        institutions = ticker.get_institutional_holders().to_json(orient='records')
        if institutions == '[]':
            return None
        return institutions
    except Exception as e:
        print(f"Error fetching institutional holders for {symbol}: {e}")


def get_insider_transactions(symbol):
    ticker = yf.Ticker(symbol)
    try:
        insider_transactions = ticker.get_insider_transactions().to_json(orient='records')
        if insider_transactions == '[]':
            return None
        return insider_transactions
    except Exception as e:
        print(f"Error fetching insider transactions for {symbol}: {e}")
