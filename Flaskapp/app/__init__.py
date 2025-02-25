from flask import Flask, current_app
from config import DevConfig, ProdConfig
from flask_sqlalchemy import SQLAlchemy
from flask_migrate import Migrate
from flask_login import LoginManager


db = SQLAlchemy()
migrate = Migrate()
login_manager = LoginManager()
# view function that handles logins
login_manager.login_view = 'auth.login'


# application factory
def create_app(config_class=ProdConfig):
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
            from app.main.forms import SearchForm
            search_form = SearchForm()
            return dict(search_form=search_form)

    # blueprints
    from app.errors import errors
    from app.auth import bp_auth
    from app.feed import bp_feed
    from app.main import bp_main
    from app.users import bp_users
    from app.stocks import bp_stocks
    from app.options import bp_options
    from app.broad import bp_broad
    from app.macro import bp_macro

    app.register_blueprint(errors)
    app.register_blueprint(bp_auth)
    app.register_blueprint(bp_feed)
    app.register_blueprint(bp_main)
    app.register_blueprint(bp_users)
    app.register_blueprint(bp_stocks)
    app.register_blueprint(bp_options)
    app.register_blueprint(bp_broad)
    app.register_blueprint(bp_macro)

    return app
