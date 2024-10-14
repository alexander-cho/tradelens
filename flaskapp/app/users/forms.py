from flask_wtf import FlaskForm
from wtforms import StringField, SubmitField, TextAreaField
from wtforms.validators import ValidationError, DataRequired, Length

from ..models import User


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
    """
    Used for instances like clicking follow/unfollow button for a particular user (send POST request)
    """
    submit = SubmitField('Submit')
