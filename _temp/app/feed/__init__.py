from flask import Blueprint

bp_feed = Blueprint('feed', __name__, template_folder='templates')

from . import routes
