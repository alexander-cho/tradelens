from flask import render_template

from modules.providers.federalreserve import FederalReserve

from . import bp_macro


@bp_macro.route('/macro/general', methods=['GET'])
def general():
    federal_reserve = FederalReserve()

    # general
    gdp = federal_reserve.get_quarterly_gdp()

    return render_template('macro/general_economy.html',
                           title='Macro - Economy',
                           gdp=gdp)


@bp_macro.route('/macro/inflation', methods=['GET'])
def inflation():
    federal_reserve = FederalReserve()
    cpi = federal_reserve.get_cpi()

    return render_template('macro/inflation.html',
                           title='Macro - Inflation',
                           cpi=cpi)


@bp_macro.route('/macro/labor-market', methods=['GET'])
def labor_market():
    federal_reserve = FederalReserve()
    unemployment_rate = federal_reserve.get_unemployment_rate()
    total_nonfarm_payroll = federal_reserve.get_payroll()

    return render_template('macro/labor_market.html',
                           title='Macro - Labor Market',
                           unemployment_rate=unemployment_rate,
                           total_nonfarm_payroll=total_nonfarm_payroll)


@bp_macro.route('/macro/financial-markets', methods=['GET'])
def financial_markets():
    federal_reserve = FederalReserve()
    interest_rates = federal_reserve.get_interest_rates()
    ten_year_yield = federal_reserve.get_10yr()

    return render_template('macro/financial_markets.html',
                           title='Macro - Financial Markets',
                           interest_rates=interest_rates,
                           ten_year_yield=ten_year_yield)


@bp_macro.route('/macro/trade', methods=['GET'])
def trade():
    federal_reserve = FederalReserve()
    trade_deficit = federal_reserve.get_trade_balance()

    return render_template('macro/trade.html',
                           title='Macro - Trade',
                           trade_deficit=trade_deficit,)


@bp_macro.route('/macro/industrial-activity', methods=['GET'])
def industrial_activity():
    federal_reserve = FederalReserve()
    capacity_utilization = federal_reserve.get_capacity_utilization()

    return render_template('macro/industrial_activity.html',
                           title='Macro - Industrial Activity',
                           capacity_utilization=capacity_utilization)


@bp_macro.route('/macro/housing-market', methods=['GET'])
def housing_market():
    federal_reserve = FederalReserve()
    housing_data = federal_reserve.get_housing_data()

    return render_template('macro/housing_market.html',
                           title='Macro - Housing Market',
                           housing_data=housing_data)


@bp_macro.route('/macro/commodities', methods=['GET'])
def commodities():
    federal_reserve = FederalReserve()
    commodities = federal_reserve.get_commodities()

    return render_template('macro/commodities.html',
                           title='Macro - Commodities',
                           commodities=commodities)
