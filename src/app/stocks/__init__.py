from flask import Blueprint

bp_stocks = Blueprint('stocks', __name__, template_folder='templates')

from . import routes
