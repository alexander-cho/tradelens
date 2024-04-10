from flask_wtf import FlaskForm
from flask_wtf.form import _Auto
from wtforms import StringField, PasswordField, BooleanField, SubmitField, TextAreaField
from wtforms.validators import ValidationError, DataRequired, Email, EqualTo, Length
from wtforms.widgets import TextArea
# from flask_ckeditor import CKEditorField
# from flask_wtf.file import FileField
from app.models import User


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
        user = User.query.filter_by(username = username.data).first() # check if the username exists in the database
        if user is not None:
            raise ValidationError("Username already registered, please use a different username")
        
    def validate_email(self, email):
        user = User.query.filter_by(email = email.data).first() # check if the email exists in the database
        if user is not None:
            raise ValidationError("Email already registered, please use a different email address")


# create a form for user to edit profile
class EditProfileForm(FlaskForm):
    username = StringField('Username', validators=[DataRequired()])
    about_me = TextAreaField('About me', validators=[Length(min=0, max=200)])
    submit = SubmitField('Confirm changes')

    def __init__(self, original_username, *args, **kwargs):
        super().__init__(*args, **kwargs)
        self.original_username = original_username

    def validate_username(self, username):
        if username.data != self.original_username:
            user = User.query.filter_by(username = username.data).first()
            if user is not None:
                raise ValidationError("That username is already taken. Please choose a different username")
            

# empty form
class EmptyForm(FlaskForm):
    submit = SubmitField('Submit')


# posts form
class PostForm(FlaskForm):
    title = StringField("Title", validators=[DataRequired()])
    content = StringField("Content", validators=[DataRequired()], widget=TextArea())
    # content = CKEditorField("Content", validators=[DataRequired()])
    # author = StringField("Author")
    # slug = StringField("Slug", validators=[DataRequired()])
    submit = SubmitField("Submit")


# class name_form(FlaskForm):
#     name = StringField("What is your name? ", validators=[DataRequired()])
#     submit = SubmitField("Submit")

# class password_form(FlaskForm):
#     email = StringField("What is your email? ", validators=[DataRequired()])
#     password_hash = PasswordField("What is your password? ", validators=[DataRequired()])
#     submit = SubmitField("Submit")

# create a search form
class SearchForm(FlaskForm):
    searched = StringField("Searched", validators=[DataRequired()]) # name="searched" attribute, in form of navbar.html
    submit = SubmitField("Submit")