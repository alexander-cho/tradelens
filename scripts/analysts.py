import yfinance as yf


def get_analyst_ratings(symbol):
    """
    because there seem to be repeated firm ratings, we get the most recent ones (first occurrences) only
    """
    ticker = yf.Ticker(symbol)
    # index of original df is 'GradeDate' which we want to include
    analyst_ratings_df = ticker.get_upgrades_downgrades().reset_index()
    analyst_ratings_dict = analyst_ratings_df.to_dict('records')

    ratings_by_unique_firms = {}

    for rating in analyst_ratings_dict:
        firm_name = rating['Firm']
        if firm_name not in ratings_by_unique_firms:
            # If the firm is not already seen, add it to the unique_ratings dictionary
            ratings_by_unique_firms[firm_name] = rating

    return ratings_by_unique_firms


print(get_analyst_ratings('SOFI'))
