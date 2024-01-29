from flask import Flask, render_template, flash, request, url_for, redirect
from flask_wtf import FlaskForm
from wtforms import StringField, SubmitField, PasswordField, BooleanField, ValidationError
from wtforms.validators import DataRequired, EqualTo, Length
from flask_sqlalchemy import SQLAlchemy
from flask_migrate import Migrate
from datetime import datetime
from werkzeug.security import generate_password_hash, check_password_hash
from datetime import date
from wtforms.widgets import TextArea


# create a flask instance
app = Flask(__name__)
# add database
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///tradelens.db'

# secret key
app.config['SECRET_KEY'] = "e07b43t"
# initialize database
db = SQLAlchemy(app)
# migrate database
migrate = Migrate(app, db)

# create posts model
class Posts(db.Model):
    id = db.Column(db.Integer, primary_key = True)
    title = db.Column(db.String(255))
    content = db.Column(db.Text)
    author = db.Column(db.String(255))
    date_posted = db.Column(db.DateTime, default=datetime.utcnow)
    slug = db.Column(db.String(255))

# posts form
class PostForm(FlaskForm):
    title = StringField("Title", validators=[DataRequired()])
    content = StringField("Content", validators=[DataRequired()], widget=TextArea())
    author = StringField("Author", validators=[DataRequired()])
    slug = StringField("Slug", validators=[DataRequired()])
    submit = SubmitField("Submit")

# delete a post
@app.route('/posts/delete/<int:id>')
def delete_post(id):
    post_to_delete = Posts.query.get_or_404(id)
    try:
        db.session.delete(post_to_delete)
        db.session.commit()
        flash("Post has been deleted")
        return render_template("posts.html", post_to_delete = post_to_delete)
    except:
        # return error message
        flash("Could not delete post")
        return render_template("posts.html", post_to_delete = post_to_delete)


@app.route('/posts') #this will potentially become home page content
def posts():
    # grab all posts from the database
    posts = Posts.query.order_by(Posts.date_posted) # let's query by chronological order, from the Posts model.
    return render_template("posts.html", posts = posts)

@app.route('/posts/<int:id>')
def post(id):
    post = Posts.query.get_or_404(id)
    return render_template("post.html", post = post)

@app.route('/posts/edit/<int:id>', methods=['GET', 'POST'])
def edit_post(id):
    post = Posts.query.get_or_404(id)
    form = PostForm()
    if form.validate_on_submit():
        # Update the post attributes with the new data once edit submission is validated
        post.title = form.title.data
        post.author = form.author.data
        post.slug = form.slug.data
        post.content = form.content.data
        # update database with modifications
        db.session.add(post)
        db.session.commit()
        flash("Post has been updated")
        return redirect(url_for('post', id=post.id)) # redirect back to singular post page
    
    # Populate the form fields with current values of the post
    form.title.data = post.title
    form.author.data = post.author
    form.slug.data = post.slug
    form.content.data = post.content
    return render_template("edit_post.html", form = form) # goes back to newly edited singular post page


# add posts page
@app.route('/add_post', methods=['GET', 'POST'])
def add_post():
    form = PostForm()
    if form.validate_on_submit():
        post = Posts(title=form.title.data, content=form.content.data, author=form.author.data, slug=form.slug.data)
        # clear form
        form.title.data = ''
        form.content.data = ''
        form.author.data = ''
        form.slug.data = ''
        #add post to database
        db.session.add(post)
        db.session.commit()

        flash("Your post has been submitted")

    return render_template("add_post.html", form=form)


# webpage to return JSON (jsonify)
@app.route('/date')
def get_current_date():
    favorite_companies = {
        "Alex": "SoFi Techs",
        "Chris": "Paypal Hold",
        "Tim": "Affirm"
    }
    return favorite_companies
    #return {"Date": date.today()}


# create user model
class Users(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String(200), nullable=False)
    email = db.Column(db.String(100), nullable=False, unique=True)
    favorite_stock = db.Column(db.String(10))
    date_added = db.Column(db.DateTime, default=datetime.utcnow)
    # passwords
    password_hash = db.Column(db.String(128))

    @property
    def password(self):
        raise AttributeError('Password is not a a readable attribute')
    
    @password.setter
    def password(self, password):
        self.password_hash = generate_password_hash(password)

    def verify_password(self, password):
        return check_password_hash(self.password_hash, password) 

    # create string representiation
    def __repr__(self) -> str:
        return '<Name %r>' % self.name

# delete a database record
@app.route('/delete/<int:id>')
def delete(id):
    user_to_delete = Users.query.get_or_404(id)
    name = None
    form = user_form()
    try:
        db.session.delete(user_to_delete)
        db.session.commit()
        flash("User deleted successfully")
        our_users = Users.query.order_by(Users.date_added)
        return render_template("add_user.html", form=form, name=name, our_users=our_users)
    except:
        flash("Could not delete user")
        return render_template("add_user.html", form=form, name=name, our_users=our_users)

# create a form class
class user_form(FlaskForm):
    name = StringField("Name", validators=[DataRequired()])
    email = StringField("Email", validators=[DataRequired()])
    favorite_stock = StringField("Favorite Stock") # no validator, ok if blank
    password_hash = PasswordField("Password", validators=[DataRequired(), EqualTo('password_hash2', message="Passwords must match")])
    password_hash2 = PasswordField("Confirm Password", validators=[DataRequired()])
    submit = SubmitField("Submit")

# update a database record
@app.route('/update/<int:id>', methods=['GET', 'POST'])
def update(id):
    form = user_form()
    name_to_update = Users.query.get_or_404(id) # query the Users table, get or if it doesnt exists, pass in the id, which comes in from the <int: id> url
    if request.method == 'POST':
        name_to_update.name = request.form['name']
        name_to_update.email = request.form['email']
        name_to_update.favorite_stock = request.form['favorite_stock']
        try:
            db.session.commit()
            flash('User updated successfully')
            return render_template("update.html", form=form, name_to_update=name_to_update)
        except:
            flash('Could not update user')
            return render_template("update.html", form=form, name_to_update=name_to_update)
    else:
        return render_template("update.html", form=form, name_to_update=name_to_update)


class name_form(FlaskForm):
    name = StringField("What is your name? ", validators=[DataRequired()])
    submit = SubmitField("Submit")

class password_form(FlaskForm):
    email = StringField("What is your email? ", validators=[DataRequired()])
    password_hash = PasswordField("What is your password? ", validators=[DataRequired()])
    submit = SubmitField("Submit")

# add a user to database
@app.route('/user/add', methods=['GET', 'POST'])
def add_user():
    name = None
    form = user_form()
    if form.validate_on_submit():
        user = Users.query.filter_by(email=form.email.data).first() # this should return None, since all email addresses are unique
        if user is None:
            # Hash the password
            hashed_pw = generate_password_hash(form.password_hash.data)
            user = Users(name = form.name.data, email = form.email.data, favorite_stock = form.favorite_stock.data, password_hash=hashed_pw )
            db.session.add(user)
            db.session.commit()
        name = form.name.data
        form.name.data = ''
        form.email.data = ''
        form.favorite_stock.data = ''
        form.password_hash.data = ''
        flash('User added')
    our_users = Users.query.order_by(Users.date_added) # chronological order of account created
    return render_template("add_user.html", form=form, name=name, our_users=our_users)

@app.route('/')
def index():
    alex_name = "alex"
    stock_list = ["SOFI", "LC", "AFRM", "UPST", 41]
    return render_template("index.html", alex_name=alex_name, stock_list=stock_list)

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

# Create password test page
@app.route('/test_pw', methods=['GET', 'POST'])
def test_pw():
    email = None
    password = None
    pw_to_check = None
    passed = None
    form = password_form()
    # Validate form
    if form.validate_on_submit():
        email = form.email.data # assign to whatever user inputs
        password = form.password_hash.data

        form.email.data = '' # clear for next user
        form.password_hash.data = ''

        #lookup user by email address
        pw_to_check = Users.query.filter_by(email=email).first()

        #check hash pw
        passed = check_password_hash(pw_to_check.password_hash, password)
    return render_template("test_pw.html", email=email, password=password, pw_to_check=pw_to_check, passed=passed, form=form)

# Create name page
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