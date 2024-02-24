# from datetime import datetime
# from flask_login import UserMixin
# from werkzeug.security import generate_password_hash, check_password_hash
# from flask_sqlalchemy import SQLAlchemy
from typing import Optional
import sqlalchemy as sa
import sqlalchemy.orm as so
from app import db



# create user model
class User(db.Model):
    id: so.Mapped[int] = so.mapped_column(primary_key=True)
    username: so.Mapped[str] = so.mapped_column(sa.String(64), index=True, unique=True)
    email: so.Mapped[str] = so.mapped_column(sa.String(120), index=True, unique=True)
    password_hash: so.Mapped[Optional[str]] = so.mapped_column(sa.String(256))
    # favorite_stock = db.Column(db.String(10))
    # about_author = db.Column(db.Text(), nullable=True)
    # date_added = db.Column(db.DateTime, default=datetime.utcnow)
    # profile_pic = db.Column(db.String(), nullable=True)
    # # passwords
    # password_hash = db.Column(db.String(128))
    # # User can have many posts
    # posts = db.relationship('Posts', backref = 'poster') # uppercase since referencing the Posts class, not a call to the database
    def __repr__(self) -> str:
        return '<User {}>'.format(self.username)

# # initialize database
# db = SQLAlchemy() #(now in __init__.py)

# # create posts model
# class Posts(db.Model):
#     id = db.Column(db.Integer, primary_key=True)
#     title = db.Column(db.String(255))
#     content = db.Column(db.Text)
#     # author = db.Column(db.String(255))
#     date_posted = db.Column(db.DateTime, default=datetime.utcnow)
#     slug = db.Column(db.String(255))
#     # create a foreign key to link users which will refer to the primary key from the Users model
#     poster_id = db.Column(db.Integer, db.ForeignKey('users.id')) # lowercase, referring to the table



#     @property
#     def password(self):
#         raise AttributeError('Password is not a a readable attribute')
    
#     @password.setter
#     def password(self, password):
#         self.password_hash = generate_password_hash(password)

#     def verify_password(self, password):
#         return check_password_hash(self.password_hash, password) 

#     # create string representiation
#     def __repr__(self) -> str:
#         return '<Name %r>' % self.name
    
# # create stocks model
# class Stocks(db.Model):
#     id = db.Column(db.Integer, primary_key=True)
#     ticker_symbol = db.Column(db.String(10), nullable=False, unique=True)
#     company_name = db.Column(db.String(100), nullable=False)
#     open = db.Column(db.Float(), nullable=True)
#     high = db.Column(db.Float(), nullable=True)
#     low = db.Column(db.Float(), nullable=True)
#     close = db.Column(db.Float(), nullable=True)
#     volume = db.Column(db.Float(), nullable=True)
#     institutional_info = db.Column(db.Text(), nullable=True)
#     last_price = db.Column(db.Float(), nullable=True)