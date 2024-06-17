from flask import render_template

from modules.providers.polygon_ import Polygon

from . import bp_screener


@bp_screener.route('/technical-screener')
def technical_screener():
    polygon = Polygon(
        ticker='SOFI',
        multiplier=1,
        timespan='hour',
        from_='2024-06-10',
        to='2024-06-14',
        limit=50000
    )
    plot = polygon.create_symbol_plot()
    return render_template('screener/technical_screener.html',
                           title='Technical Screener',
                           plot=plot)
