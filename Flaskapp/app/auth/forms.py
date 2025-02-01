from flask_wtf import FlaskForm
from wtforms import StringField, PasswordField, BooleanField, SubmitField
from wtforms.validators import ValidationError, DataRequired, Email, EqualTo

from ..models import User


# create login form
class LoginForm(FlaskForm):
    username = StringField("Username", validators=[DataRequired()])
    password = PasswordField("Password", validators=[DataRequired()])
    remember_me = BooleanField("Remember me")
    submit = SubmitField("Log in")


# create registration form
class RegistrationForm(FlaskForm):
    username = StringField("Username", validators=[DataRequired()])
    email = StringField("Email", validators=[DataRequired(), Email()])
    password_hash = PasswordField("Password", validators=[DataRequired()])
    password_hash2 = PasswordField("Confirm Password", validators=[DataRequired(), EqualTo('password_hash', message="Passwords must match")])
    submit = SubmitField("Register")

    def validate_username(self, username):
        # check if the username exists in the database
        user = User.query.filter_by(username=username.data).first()
        if user is not None:
            raise ValidationError("Username already registered, please use a different username")

    def validate_email(self, email):
        # check if the email exists in the database
        user = User.query.filter_by(email=email.data).first()
        if user is not None:
            raise ValidationError("Email already registered, please use a different email address")
