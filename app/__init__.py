from flask import Flask
from config import Config
from flask_sqlalchemy import SQLAlchemy
from flask_migrate import Migrate
from flask_login import LoginManager


# create a flask instance
app = Flask(__name__)
app.config.from_object(Config)

# from app.errors import bp as errors_bp
# app.register_blueprint(errors_bp)

db = SQLAlchemy(app)

migrate = Migrate(app, db)

login_manager = LoginManager(app)

# view function that handles logins
login_manager.login_view = 'login'

from app import routes, models
