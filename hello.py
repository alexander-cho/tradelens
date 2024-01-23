from flask import Flask, render_template, flash
from flask_wtf import FlaskForm
from wtforms import StringField, SubmitField
from wtforms.validators import DataRequired
from flask_sqlalchemy import SQLAlchemy
from datetime import datetime

# create a flask instance
app = Flask(__name__)
# add database
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///tradelens.db'
# secret key
app.config['SECRET_KEY'] = "e07b43t"
# initialize database
db = SQLAlchemy(app)

# create model
class Users(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String(200), nullable=False)
    email = db.Column(db.String(100), nullable=False, unique=True)
    date_added = db.Column(db.DateTime, default=datetime.utcnow)

    # create string representiation
    def __repr__(self) -> str:
        return '<Name %r>' % self.name

# create a form class
class user_form(FlaskForm):
    name = StringField("Name", validators=[DataRequired()])
    email = StringField("Email", validators=[DataRequired()])
    submit = SubmitField("Submit")


class name_form(FlaskForm):
    name = StringField("What is your name? ", validators=[DataRequired()])
    submit = SubmitField("Submit")

@app.route('/user/add', methods=['GET', 'POST'])
def add_user():
    name = None
    form = user_form()
    if form.validate_on_submit():
        user = Users.query.filter_by(email=form.email.data).first() # this should return None, since all email addresses are unique
        if user is None:
            user = Users(name=form.name.data, email=form.email.data)
            db.session.add(user)
            db.session.commit()
        name = form.name.data
        form.name.data = ''
        form.email.data = ''
        flash('User added')
    our_users = Users.query.order_by(Users.date_added)
    return render_template("add_user.html", form=form, name=name, our_users=our_users)

@app.route('/')
def index():
    alex_name = "alex"
    favorite_stocks = ["SOFI", "LC", "AFRM", "UPST", 41]
    return render_template("index.html", alex_name=alex_name, favorite_stocks=favorite_stocks)

@app.route('/user/<name>')
def user(name):
    return render_template("user.html", name=name)

# custom error pages

# invalid URL
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
    # Validate form
    if form.validate_on_submit():
        name = form.name.data # assign to whatever user inputs
        form.name.data = '' # clear for next user
        flash("Form submitted successfully")
    return render_template("name.html", name=name, form=form)


if __name__ == '__main__':
    app.run(debug=True)