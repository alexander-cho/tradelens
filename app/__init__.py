from flask import Flask
from config import Config


app = Flask(__name__) # create a flask instance
app.config.from_object(Config)

from app import routes