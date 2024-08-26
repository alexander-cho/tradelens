from flask import Flask, current_app
from src.config import Config
from flask_sqlalchemy import SQLAlchemy
from flask_migrate import Migrate
from flask_login import LoginManager


db = SQLAlchemy()
migrate = Migrate()
login_manager = LoginManager()
# view function that handles logins
login_manager.login_view = 'auth.login'


# application factory
def create_app(config_class=Config):
    # create a flask instance
    app = Flask(__name__)
    app.config.from_object(config_class)

    db.init_app(app)
    migrate.init_app(app, db)
    login_manager.init_app(app)

    # inject search form into all templates
    with app.app_context():
        @current_app.context_processor
        def inject_search_form():
            from src.app.main.forms import SearchForm
            search_form = SearchForm()
            return dict(search_form=search_form)

    # blueprints
    from src.app.errors import errors
    from src.app.auth import bp_auth
    from src.app.feed import bp_feed
    from src.app.main import bp_main
    from src.app.users import bp_users
    from src.app.stocks import bp_stocks
    from src.app.options import bp_options
    from src.app.broad import bp_broad
    from src.app.macro import bp_macro
    from src.app.screener import bp_screener
    app.register_blueprint(errors)
    app.register_blueprint(bp_auth)
    app.register_blueprint(bp_feed)
    app.register_blueprint(bp_main)
    app.register_blueprint(bp_users)
    app.register_blueprint(bp_stocks)
    app.register_blueprint(bp_options)
    app.register_blueprint(bp_broad)
    app.register_blueprint(bp_macro)
    app.register_blueprint(bp_screener)

    return app
