import os
from dotenv import load_dotenv

basedir = os.path.abspath(os.path.dirname(__file__))

load_dotenv()

# configuration variables for app
class Config:
    UPLOAD_FOLDER = 'static/images'
    SECRET_KEY = os.environ.get('SECRET_KEY') or 'you_will_never_guess'


class ProdConfig(Config):
    SQLALCHEMY_DATABASE_URI = os.getenv('DATABASE_URL')


class DevConfig(Config):
    SQLALCHEMY_DATABASE_URI = 'sqlite:///' + os.path.join(basedir, 'app.db')


class TestConfig(Config):
    TESTING = True
    SQLALCHEMY_DATABASE_URI = 'sqlite:///:memory:'
    SQLALCHEMY_TRACK_MODIFICATIONS = False
    # Disable CSRF for testing
    WTF_CSRF_ENABLED = False
