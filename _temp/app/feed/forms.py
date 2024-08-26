from flask_wtf import FlaskForm
from wtforms import StringField, SubmitField
from wtforms.validators import DataRequired
from wtforms.widgets import TextArea
# from flask_wtf.file import FileField


# posts form
class PostForm(FlaskForm):
    title = StringField("Title", validators=[DataRequired()])
    content = StringField("Content", validators=[DataRequired()], widget=TextArea())
    # author = StringField("Author")
    # slug = StringField("Slug", validators=[DataRequired()])
    submit = SubmitField("Submit")
