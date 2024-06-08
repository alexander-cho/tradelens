from flask import Blueprint

bp_broad = Blueprint('broad', __name__)

from . import routes
