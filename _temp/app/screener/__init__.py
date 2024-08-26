from flask import Blueprint

bp_screener = Blueprint('screener', __name__, template_folder='templates')

from . import routes
