from flask import Blueprint

bp_options = Blueprint('options', __name__)

from . import routes
