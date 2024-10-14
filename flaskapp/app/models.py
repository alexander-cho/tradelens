from app import login_manager
from flask_login import UserMixin
from werkzeug.security import generate_password_hash, check_password_hash
from typing import Optional
import sqlalchemy as sa
import sqlalchemy.orm as so
from app import db
from datetime import datetime, timezone
from hashlib import md5


# allow Flask-Login to manage user sessions and authentication
@login_manager.user_loader
def load_user(id):  # Flask-Login passes id argument as string
    return db.session.get(User, int(id))


# followers association table which User model will reference
followers = sa.Table(
    'followers',
    db.metadata,
    sa.Column('follower_id', sa.Integer, sa.ForeignKey('user.id'), primary_key=True),
    sa.Column('following_id', sa.Integer, sa.ForeignKey('user.id'), primary_key=True)
    # both foreign keys marked as primary keys since the pair creates a unique combination
)


# create user model
class User(db.Model, UserMixin):
    id: so.Mapped[int] = so.mapped_column(primary_key=True)
    username: so.Mapped[str] = so.mapped_column(sa.String(64), index=True, unique=True)
    email: so.Mapped[str] = so.mapped_column(sa.String(120), index=True, unique=True)
    password_hash: so.Mapped[Optional[str]] = so.mapped_column(sa.String(256))
    about_me: so.Mapped[Optional[str]] = so.mapped_column(sa.String(200))
    last_seen: so.Mapped[Optional[datetime]] = so.mapped_column(default=lambda: datetime.now(timezone.utc))
    date_joined: so.Mapped[Optional[datetime]] = so.mapped_column(index=True, default=lambda: datetime.now(timezone.utc))
    # User can have many posts
    posts: so.WriteOnlyMapped['Post'] = so.relationship(back_populates='author')

    def set_password(self, password):
        self.password_hash = generate_password_hash(password)

    def check_password(self, password):
        return check_password_hash(self.password_hash, password)  # compare hash with user entered value

    def __repr__(self) -> str:
        return '<User {}>'.format(self.username)

    def avatar(self, size):
        digest = md5(self.email.lower().encode('utf-8')).hexdigest()
        return f'https://www.gravatar.com/avatar/{digest}?d=identicon&s={size}'  # url of user's avatar image

    # followers relationship
    following: so.WriteOnlyMapped['User'] = so.relationship(
        secondary=followers,
        primaryjoin=(followers.c.follower_id == id),
        secondaryjoin=(followers.c.following_id == id),
        back_populates='followers'
    )
    followers: so.WriteOnlyMapped['User'] = so.relationship(
        secondary=followers,
        primaryjoin=(followers.c.following_id == id),
        secondaryjoin=(followers.c.follower_id == id),
        back_populates='following'
    )

    def is_following(self, user) -> bool:
        # select rows from following relationship associated with current user where
        # id of the user in the following relationship == id of user passed as param
        query = self.following.select().where(User.id == user.id)
        return db.session.scalar(query) is not None

    def follow(self, user):
        # if they aren't following yet
        if not self.is_following(user):
            self.following.add(user)

    def unfollow(self, user):
        if self.is_following(user):
            self.following.remove(user)

    def followers_count(self):
        num_followers = sa.select(sa.func.count()).select_from(self.followers.select().subquery())
        return db.session.scalar(num_followers)

    def following_count(self):
        num_following = sa.select(sa.func.count()).select_from(self.following.select().subquery())
        return db.session.scalar(num_following)

    def following_posts(self):
        Author = so.aliased(User)
        Follower = so.aliased(User)
        return (
            sa.select(Post)
            .join(Post.author.of_type(Author))
            .join(Author.followers.of_type(Follower), isouter=True)
            .where(sa.or_(
                Follower.id == self.id,
                Author.id == self.id  # get own posts as well
            ))
            .group_by(Post)
            .order_by(Post.timestamp.desc())  # get most recent posts first
        )


# create post model
class Post(db.Model):
    id: so.Mapped[int] = so.mapped_column(primary_key=True)
    title: so.Mapped[str] = so.mapped_column(sa.String(10), index=True)
    content: so.Mapped[str] = so.mapped_column(sa.Text())
    timestamp: so.Mapped[datetime] = so.mapped_column(index=True, default=lambda: datetime.now(timezone.utc))

    # foreign key to link user_id which refers to the primary key id from the User model
    user_id: so.Mapped[int] = so.mapped_column(sa.ForeignKey(User.id), index=True)

    author: so.Mapped['User'] = so.relationship(back_populates='posts')

    # create string representation
    def __repr__(self) -> str:
        return '<Post {}>'.format(self.content)


# create stock model
class Stock(db.Model):
    id: so.Mapped[int] = so.mapped_column(primary_key=True)
    ticker_symbol: so.Mapped[str] = so.mapped_column(sa.String(10), index=True, unique=True)
    company_name: so.Mapped[str] = so.mapped_column(sa.String(100), index=True)
