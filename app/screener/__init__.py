from flask import Blueprint

bp_screener = Blueprint('screener', __name__)

from . import routes
