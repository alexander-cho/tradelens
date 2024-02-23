from flask_wtf import FlaskForm
from wtforms import StringField, PasswordField, BooleanField, SubmitField#, ValidationError, TextAreaField
from wtforms.validators import DataRequired#, EqualTo, Length
# from wtforms.widgets import TextArea
# from flask_ckeditor import CKEditorField
# from flask_wtf.file import FileField



# create login form
class LoginForm(FlaskForm):
    username = StringField("Username", validators=[DataRequired()])
    password = PasswordField("Password", validators=[DataRequired()])
    remember_me = BooleanField("Remember me")
    submit = SubmitField("Submit")

# # posts form
# class PostForm(FlaskForm):
#     title = StringField("Title", validators=[DataRequired()])
#     # content = StringField("Content", validators=[DataRequired()], widget=TextArea())
#     content = CKEditorField("Content", validators=[DataRequired()])
#     author = StringField("Author")
#     slug = StringField("Slug", validators=[DataRequired()])
#     submit = SubmitField("Submit")

# # create a form class
# class user_form(FlaskForm):
#     name = StringField("Name", validators=[DataRequired()])
#     username = StringField("Username", validators=[DataRequired()])
#     email = StringField("Email", validators=[DataRequired()])
#     favorite_stock = StringField("Favorite Stock") # no validator, ok if blank
#     about_author = TextAreaField("About Author")
#     password_hash = PasswordField("Password", validators=[DataRequired(), EqualTo('password_hash2', message="Passwords must match")])
#     password_hash2 = PasswordField("Confirm Password", validators=[DataRequired()])
#     profile_pic = FileField("Profile Pic")
#     submit = SubmitField("Submit")

# class name_form(FlaskForm):
#     name = StringField("What is your name? ", validators=[DataRequired()])
#     submit = SubmitField("Submit")

# class password_form(FlaskForm):
#     email = StringField("What is your email? ", validators=[DataRequired()])
#     password_hash = PasswordField("What is your password? ", validators=[DataRequired()])
#     submit = SubmitField("Submit")

# # create a search form
# class SearchForm(FlaskForm):
#     searched = StringField("Searched", validators=[DataRequired()])
#     submit = SubmitField("Submit")