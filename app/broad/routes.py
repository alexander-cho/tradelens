from flask import render_template, flash, redirect, url_for

from modules.providers.alphavantage import AlphaVantage
from modules.providers.finnhub_ import Finnhub
from modules.providers.federalreserve import FederalReserve

from modules.utils.date_ranges import get_date_range_ahead

from . import bp_broad


# return IPOs anticipated in the next 3 months, upcoming earnings calendar
@bp_broad.route('/earnings-ipos', methods=['GET'])
def earnings_ipos():
    alphavantage = AlphaVantage()
    finnhub = Finnhub()

    ipo_data = alphavantage.get_ipos_data()

    (today, future_date) = get_date_range_ahead(days_ahead=7)
    earnings_calendar = finnhub.get_earnings_calendar(_from=today, to=future_date)

    return render_template('broad/earnings_ipos.html',
                           title='IPOs',
                           ipo_data=ipo_data,
                           earnings_calendar=earnings_calendar)


@bp_broad.route('/macro')
def macro():
    federal_reserve = FederalReserve()

    gdp = federal_reserve.get_quarterly_gdp()
    cpi = federal_reserve.get_cpi()

    return render_template('broad/macro.html',
                           title='Macro',
                           gdp=gdp,
                           cpi=cpi)


@bp_broad.route('/market-news/<category>', methods=['GET'])
def market_news(category):
    category = category.lower()
    # market news categories
    valid_categories = ['general', 'forex', 'crypto', 'merger']
    if category not in valid_categories:
        flash(f"Invalid news category: {category}. Showing general news instead.")
        category = 'general'
        return redirect(url_for('broad.market_news', category=category))

    finnhub = Finnhub()
    try:
        news = finnhub.get_market_news(category)
    except ValueError as e:
        flash(str(e))
        return redirect(url_for('broad.market_news', category='general'))

    return render_template('broad/market_news.html',
                           title=f"{category.capitalize()} News",
                           category=category,
                           news=news)
