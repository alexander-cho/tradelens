import os

class Config:
    SECRET_KEY = os.environ.get('SECRET_KEY') or 'e07b43t'