from flask import render_template

from modules.providers.polygon_ import Polygon

from . import bp_screener

from .patterns import sofi


@bp_screener.route('/technical-screener')
def technical_screener():
    sofi_df = sofi
    return render_template(
        template_name_or_list='screener/technical_screener.html',
        title='Technical Screener',
        sofi_df=sofi_df
    )
