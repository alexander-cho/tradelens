# from datetime import datetime
# from flask_login import UserMixin
# from werkzeug.security import generate_password_hash, check_password_hash
# from flask_sqlalchemy import SQLAlchemy
from typing import Optional
import sqlalchemy as sa
import sqlalchemy.orm as so
from app import db
from datetime import datetime, timezone



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
    # User can have many posts
    posts: so.WriteOnlyMapped['Post'] = so.relationship(back_populates='author')

    def __repr__(self) -> str:
        return '<User {}>'.format(self.username)
    
# create post model
class Post(db.Model):
    id: so.Mapped[int] = so.mapped_column(primary_key=True)
    title: so.Mapped[str] = so.mapped_column(sa.String(255), index=True)
    content: so.Mapped[str] = so.mapped_column(sa.Text())
    # slug = db.Column(db.String(255))
    timestamp: so.Mapped[datetime] = so.mapped_column(index=True, default=lambda: datetime.now(timezone.utc))
    user_id: so.Mapped[int] = so.mapped_column(sa.ForeignKey(User.id), index=True) # foreign key to link user_id which refers to the primary key id from the User model

    author: so.Mapped[User] = so.relationship(back_populates='posts')


#     @property
#     def password(self):
#         raise AttributeError('Password is not a a readable attribute')
    
#     @password.setter
#     def password(self, password):
#         self.password_hash = generate_password_hash(password)

#     def verify_password(self, password):
#         return check_password_hash(self.password_hash, password) 

    # create string representiation
    def __repr__(self) -> str:
        return '<Post {}>'.format(self.content)

# # initialize database
# db = SQLAlchemy() #(now in __init__.py)
    
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