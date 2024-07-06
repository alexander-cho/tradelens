from flask_wtf import FlaskForm
from wtforms import StringField, SubmitField
from wtforms.validators import DataRequired


# create a search form
class SearchForm(FlaskForm):
    # name="searched" attribute, in <form> of navbar.html
    searched = StringField("Searched", validators=[DataRequired()])
    submit = SubmitField("Submit")
