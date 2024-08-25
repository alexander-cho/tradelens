import os
basedir = os.path.abspath(os.path.dirname(__file__))


# configuration variables for app
class Config:
    UPLOAD_FOLDER = 'static/images'
    SECRET_KEY = os.environ.get('SECRET_KEY') or 'you_will_never_guess'
    SQLALCHEMY_DATABASE_URI = os.environ.get('DATABASE_URL') or 'sqlite:///' + os.path.join(basedir, 'app.db')


class TestConfig(Config):
    TESTING = True
    SQLALCHEMY_DATABASE_URI = 'sqlite:///:memory:'
    SQLALCHEMY_TRACK_MODIFICATIONS = False
    WTF_CSRF_ENABLED = False  # Disable CSRF for testing
