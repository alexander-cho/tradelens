from flask import Blueprint

errors = Blueprint('errors', __name__, template_folder='templates')

from src.app.errors import errors
