from flask import current_app, render_template
from app import db

from . import errors


# custom error pages

# invalid URL
@errors.app_errorhandler(404)
def page_not_found(e):
    return render_template('errors/404.html'), 404


# internal server error
@errors.app_errorhandler(500)
def internal_error(e):
    db.session.rollback()
    return render_template('errors/500.html'), 500
