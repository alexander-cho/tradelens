from flask import Blueprint

bp_feed = Blueprint('feed', __name__)

from . import routes
