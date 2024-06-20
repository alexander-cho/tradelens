from flask import render_template

from modules.providers.federalreserve import FederalReserve

from . import bp_macro


@bp_macro.route('/macro/general', methods=['GET'])
def general():
    fed = FederalReserve()

    gdp = fed.get_gdp()
    debt_as_pct_of_gdp = fed.get_debt_as_pct_of_gdp()
    total_debt = fed.get_total_debt()
    interest_payments = fed.get_interest_payments()

    return render_template('macro/general_economy.html',
                           title='Macro - Economy',
                           gdp=gdp,
                           debt_as_pct_of_gdp=debt_as_pct_of_gdp,
                           total_debt=total_debt,
                           interest_payments=interest_payments)


@bp_macro.route('/macro/inflation', methods=['GET'])
def inflation():
    fed = FederalReserve()

    cpi = fed.get_cpi()
    ppi = fed.get_ppi()
    pce = fed.get_pce()
    disposable_income = fed.get_disposable_income()

    return render_template('macro/inflation.html',
                           title='Macro - Inflation',
                           cpi=cpi,
                           ppi=ppi,
                           pce=pce,
                           disposable_income=disposable_income)


@bp_macro.route('/macro/labor-market', methods=['GET'])
def labor_market():
    fed = FederalReserve()

    unemployment_rate = fed.get_unemployment_rate()
    total_nonfarm_payroll = fed.get_payroll()

    return render_template('macro/labor_market.html',
                           title='Macro - Labor Market',
                           unemployment_rate=unemployment_rate,
                           total_nonfarm_payroll=total_nonfarm_payroll)


@bp_macro.route('/macro/financial-markets', methods=['GET'])
def financial_markets():
    fed = FederalReserve()

    interest_rates = fed.get_interest_rates()
    ten_year_yield = fed.get_10yr()

    return render_template('macro/financial_markets.html',
                           title='Macro - Financial Markets',
                           interest_rates=interest_rates,
                           ten_year_yield=ten_year_yield)


@bp_macro.route('/macro/trade', methods=['GET'])
def trade():
    fed = FederalReserve()

    trade_deficit = fed.get_trade_balance()
    fdi = fed.get_fdi()

    return render_template('macro/trade.html',
                           title='Macro - Trade',
                           trade_deficit=trade_deficit,
                           fdi=fdi)


@bp_macro.route('/macro/industrial-activity', methods=['GET'])
def industrial_activity():
    fed = FederalReserve()

    industrial_production = fed.get_ipi()
    capacity_utilization = fed.get_capacity_utilization()

    return render_template('macro/industrial_activity.html',
                           title='Macro - Industrial Activity',
                           industrial_production=industrial_production,
                           capacity_utilization=capacity_utilization)


@bp_macro.route('/macro/housing-market', methods=['GET'])
def housing_market():
    fed = FederalReserve()

    units_started = fed.get_housing_units_started()
    median_price = fed.get_median_home_sale_price()

    return render_template('macro/housing_market.html',
                           title='Macro - Housing Market',
                           units_started=units_started,
                           median_price=median_price)


@bp_macro.route('/macro/commodities', methods=['GET'])
def commodities():
    fed = FederalReserve()

    oil_prices = fed.get_oil_prices()
    natural_gas_prices = fed.get_natural_gas_prices()
    sugar_prices = fed.get_sugar_prices()
    corn_prices = fed.get_corn_prices()

    return render_template('macro/commodities.html',
                           title='Macro - Commodities',
                           oil_prices=oil_prices,
                           natural_gas_prices=natural_gas_prices,
                           sugar_prices=sugar_prices,
                           corn_prices=corn_prices)
