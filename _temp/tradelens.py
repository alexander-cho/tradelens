from flask import current_app
import sqlalchemy as sa
import sqlalchemy.orm as so

from src.app import db
from src.app.models import User, Post

from app import db
from app.models import User, Post, Stocks


@current_app.shell_context_processor
def make_shell_context():
    return {
        'sa': sa, 
        'so': so, 
        'db': db, 
        'User': User, 
        'Post': Post,
        'Stocks': Stocks
    }
