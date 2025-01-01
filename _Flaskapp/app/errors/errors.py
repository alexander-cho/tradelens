from flask import render_template
from app import db

from . import errors


# invalid URL
@errors.app_errorhandler(404)
def page_not_found():
    return render_template('errors/templates/errors/404.html'), 404


# internal server error
@errors.app_errorhandler(500)
def internal_error():
    db.session.rollback()
    return render_template('errors/templates/errors/500.html'), 500
