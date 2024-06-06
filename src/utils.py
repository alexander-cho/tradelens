from datetime import datetime, timedelta


def get_date_range_ahead(days_ahead: int) -> tuple[str, str]:
    """
    Get the date range from today to a specified number of days in the future

    Parameters:
        days_ahead (int): number of days into the future

    Returns:
        tuple: today's date, specified future date
    """
    today = datetime.today()
    future_date = today + timedelta(days=days_ahead)

    # Format dates as strings in 'YYYY-MM-DD'
    today_str = today.strftime('%Y-%m-%d')
    future_date_str = future_date.strftime('%Y-%m-%d')

    return today_str, future_date_str


def get_date_range_past(days_past: int) -> tuple[str, str]:
    """
    Get the date range from a specified number of days in the past to today

    Parameters:
        days_past (int): number of days into the past

    Returns:
        tuple: specified past date, today's date
    """
    today = datetime.today()
    past_date = today - timedelta(days=days_past)

    # Format dates as strings in 'YYYY-MM-DD'
    today_str = today.strftime('%Y-%m-%d')
    past_date_str = past_date.strftime('%Y-%m-%d')

    return past_date_str, today_str
