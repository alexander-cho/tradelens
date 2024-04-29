from flask import render_template
from app import app, db


# custom error pages

# invalid URL
@app.errorhandler(404)
def page_not_found(e):
    return render_template('404.html'), 404


# internal server error
@app.errorhandler(500)
def internal_error(e):
    db.session.rollback()
    return render_template('500.html'), 500
