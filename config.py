import os
basedir = os.path.abspath(os.path.dirname(__file__))

# configuration variables for app
class Config:
    SECRET_KEY = os.environ.get('SECRET_KEY') or 'e07b43t'
    SQLALCHEMY_DATABASE_URI = os.environ.get('DATABASE_URL') or 'sqlite:///' + os.path.join(basedir, 'app.db')

