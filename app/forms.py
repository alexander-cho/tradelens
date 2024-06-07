from flask_wtf import FlaskForm
from wtforms import StringField, PasswordField, BooleanField, SubmitField, TextAreaField
from wtforms.validators import ValidationError, DataRequired, Email, EqualTo, Length
from wtforms.widgets import TextArea
# from flask_wtf.file import FileField
from app.models import User


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
            user = User.query.filter_by(username=username.data).first()
            if user is not None:
                raise ValidationError("That username is already taken. Please choose a different username")
            

# empty form
class EmptyForm(FlaskForm):
    submit = SubmitField('Submit')


# posts form
class PostForm(FlaskForm):
    title = StringField("Title", validators=[DataRequired()])
    content = StringField("Content", validators=[DataRequired()], widget=TextArea())
    # author = StringField("Author")
    # slug = StringField("Slug", validators=[DataRequired()])
    submit = SubmitField("Submit")


# create a search form
class SearchForm(FlaskForm):
    searched = StringField("Searched", validators=[DataRequired()])  # name="searched" attribute, in form of navbar.html
    submit = SubmitField("Submit")
