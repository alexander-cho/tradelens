from flask_wtf import FlaskForm
from wtforms import StringField, PasswordField, BooleanField, SubmitField
from wtforms.validators import ValidationError, DataRequired, Email, EqualTo
from email_validator import validate_email, EmailNotValidError  # Import the necessary functions

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
    # profile_pic = FileField("Profile Pic")
    submit = SubmitField("Register")

    def validate_username(self, username):
        user = User.query.filter_by(username=username.data).first()  # check if the username exists in the database
        if user is not None:
            raise ValidationError("Username already registered, please use a different username")

    def validate_email(self, email):
        user = User.query.filter_by(email=email.data).first()  # check if the email exists in the database
        if user is not None:
            raise ValidationError("Email already registered, please use a different email address")
        
        # Validate email format
        try:
            validate_email(email.data)
        except EmailNotValidError as e:
            raise ValidationError(str(e))
