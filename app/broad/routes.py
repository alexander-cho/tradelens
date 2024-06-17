from flask import render_template, flash, redirect, url_for

from modules.providers.alphavantage import AlphaVantage
from modules.providers.finnhub_ import Finnhub
from modules.providers.federalreserve import FederalReserve

from modules.utils.date_ranges import get_date_range_ahead

from . import bp_broad


# return IPOs anticipated in the next 3 months, upcoming earnings calendar
@bp_broad.route('/earnings-ipos', methods=['GET'])
def earnings_ipos():
    finnhub = Finnhub()

    (today, future_date) = get_date_range_ahead(days_ahead=7)
    earnings_calendar = finnhub.get_earnings_calendar(_from=today, to=future_date)
    anticipated_ipos = finnhub.get_upcoming_ipos(_from=today, to=future_date)

    return render_template('broad/earnings_ipos.html',
                           title='IPOs',
                           earnings_calendar=earnings_calendar,
                           anticipated_ipos=anticipated_ipos)


@bp_broad.route('/macro', methods=['GET'])
def macro():
    federal_reserve = FederalReserve()

    gdp = federal_reserve.get_quarterly_gdp()
    cpi = federal_reserve.get_cpi()
    interest_rates = federal_reserve.get_interest_rates()
    unemployment_rate = federal_reserve.get_unemployment_rate()
    ten_year_yield = federal_reserve.get_10yr()
    trade_deficit = federal_reserve.get_trade_balance()
    capacity_utilization = federal_reserve.get_capacity_utilization()
    total_nonfarm_payroll = federal_reserve.get_payroll()
    housing_data = federal_reserve.get_housing_data()
    commodities = federal_reserve.get_commodities()

    return render_template('broad/macro.html',
                           title='Macro',
                           gdp=gdp,
                           cpi=cpi,
                           interest_rates=interest_rates,
                           unemployment_rate=unemployment_rate,
                           ten_year_yield=ten_year_yield,
                           trade_deficit=trade_deficit,
                           capacity_utilization=capacity_utilization,
                           total_nonfarm_payroll=total_nonfarm_payroll,
                           housing_data=housing_data,
                           commodities=commodities)


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
