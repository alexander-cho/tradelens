from flask import Flask
from config import Config
from flask_sqlalchemy import SQLAlchemy
from flask_migrate import Migrate
from flask_login import LoginManager


# create a flask instance
app = Flask(__name__)
app.config.from_object(Config)

db = SQLAlchemy(app)

migrate = Migrate(app, db)

login_manager = LoginManager(app)

# view function that handles logins
login_manager.login_view = 'auth.login'


# inject search form into all templates
@app.context_processor
def inject_search_form():
    from app.main.forms import SearchForm
    search_form = SearchForm()
    return dict(search_form=search_form)


from app import models


from app.errors import errors
from app.auth import bp_auth
from app.feed import bp_feed
from app.main import bp_main
from app.users import bp_users
from app.stocks import bp_stocks
from app.options import bp_options
from app.broad import bp_broad
from app.screener import bp_screener

app.register_blueprint(errors)
app.register_blueprint(bp_auth)
app.register_blueprint(bp_feed)
app.register_blueprint(bp_main)
app.register_blueprint(bp_users)
app.register_blueprint(bp_stocks)
app.register_blueprint(bp_options)
app.register_blueprint(bp_broad)
app.register_blueprint(bp_screener)
