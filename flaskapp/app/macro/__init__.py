from flask import Blueprint

bp_macro = Blueprint('macro', __name__, template_folder='templates')

from . import routes