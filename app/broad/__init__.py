from flask import Blueprint

bp_broad = Blueprint('broad', __name__, template_folder='templates')

from . import routes
