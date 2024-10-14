from flask import render_template, flash, redirect, url_for

from modules.providers.finnhub_ import Finnhub

from modules.utils.date_ranges import get_date_range_ahead

from . import bp_broad


# anticipated IPOs, upcoming earnings calendar
@bp_broad.route('/earnings-ipos', methods=['GET'])
def earnings_ipos():
    finnhub = Finnhub()

    (earnings_today, earnings_future_date) = get_date_range_ahead(days_ahead=7)
    (ipos_today, ipos_future_date) = get_date_range_ahead(days_ahead=28)
    earnings_calendar = finnhub.get_earnings_calendar(_from=earnings_today, to=earnings_future_date)
    anticipated_ipos = finnhub.get_upcoming_ipos(_from=ipos_today, to=ipos_future_date)

    return render_template(
        template_name_or_list='broad/earnings_ipos.html',
        title='Earnings/IPOs',
        earnings_calendar=earnings_calendar,
        anticipated_ipos=anticipated_ipos
    )


# news
@bp_broad.route('/market-news/<category>', methods=['GET'])
def market_news(category):
    category = category.lower()
    # market news categories
    valid_categories = ['general', 'forex', 'crypto', 'merger']
    if category not in valid_categories:
        flash(f"Invalid news category: '{category}'. Showing general news instead.")
        category = 'general'
        return redirect(url_for('broad.market_news', category=category))

    finnhub = Finnhub()
    try:
        news = finnhub.get_market_news(category)
    except ValueError as e:
        flash(str(e))
        return redirect(url_for('broad.market_news', category='general'))

    return render_template(
        template_name_or_list='broad/market_news.html',
        title=f"{category.capitalize()} News",
        category=category,
        news=news
    )
