from flask import Blueprint

bp_users = Blueprint('users', __name__, template_folder='templates')

from . import routes
