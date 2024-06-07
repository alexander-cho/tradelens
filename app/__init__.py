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

from app import routes, models


from app.errors import errors
from app.auth import bp_auth
from app.feed import bp_feed
from app.users import bp_users

app.register_blueprint(errors)
app.register_blueprint(bp_auth)
app.register_blueprint(bp_feed)
app.register_blueprint(bp_users)
