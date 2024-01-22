from flask import Flask, render_template, flash
from flask_wtf import FlaskForm
from wtforms import StringField, SubmitField
from wtforms.validators import DataRequired


app = Flask(__name__)
app.config['SECRET_KEY'] = "e07b43t"

#create a form class
class name_form(FlaskForm):
    name = StringField("What is your name? ", validators=[DataRequired()])
    submit = SubmitField("Submit")

@app.route('/')
def index():
    alex_name = "alex"
    favorite_stocks = ["SOFI", "LC", "AFRM", "UPST", 41]
    return render_template("index.html", alex_name=alex_name, favorite_stocks=favorite_stocks)

@app.route('/user/<name>')
def user(name):
    return render_template("user.html", name=name)

# custom error pages

#invalid URL
@app.errorhandler(404)
def page_not_found(e):
    return render_template("404.html")

#internal server error
@app.errorhandler(500)
def page_not_found(e):
    return render_template("500.html")

@app.route('/name', methods=['GET', 'POST'])
def name():
    name = None
    form = name_form()
    #Validate form
    if form.validate_on_submit():
        name = form.name.data #assign to whatever user inputs
        form.name.data = '' #clear for next user
        flash("Form submitted successfully")
    return render_template("name.html", name=name, form=form)


if __name__ == '__main__':
    app.run(debug=True)