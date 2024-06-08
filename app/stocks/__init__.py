from flask import Blueprint

bp_stocks = Blueprint('stocks', __name__)

from . import routes
