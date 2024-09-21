from flask import Blueprint

bp_main = Blueprint('main', __name__, template_folder='templates')

from . import routes
