from flask import Blueprint

bp_options = Blueprint('options', __name__, template_folder='templates')

from . import routes
