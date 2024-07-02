from flask import render_template

from modules.providers.polygon_ import Polygon

from . import bp_screener


@bp_screener.route('/technical-screener')
def technical_screener():
    return render_template(
        template_name_or_list='screener/technical_screener.html',
        title='Technical Screener',
    )
