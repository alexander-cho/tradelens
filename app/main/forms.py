from flask_wtf import FlaskForm
from wtforms import StringField, SubmitField
from wtforms.validators import DataRequired


# create a search form
class SearchForm(FlaskForm):
    searched = StringField("Searched",
                           validators=[DataRequired()])  # name="searched" attribute, in form of navbar.html
    submit = SubmitField("Submit")
