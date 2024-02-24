from flask import render_template, flash, redirect, url_for#, request
# from flask_migrate import Migrate
# from werkzeug.security import generate_password_hash, check_password_hash
# from datetime import date
# from flask_login import login_user, LoginManager, login_required, logout_user, current_user
# from forms import LoginForm, PostForm, user_form, name_form, password_form, SearchForm
# from flask_ckeditor import CKEditor
# from werkzeug.utils import secure_filename
# import uuid as uuid
# import os
# from models import db, Users, Posts, Stocks

from app import app
from app.forms import LoginForm


@app.route('/')
@app.route('/index')
def index():
    user = {'username': 'Alex'} # user mock object
    posts = [ # posts mock object
        {
            'author': {'username': 'Chris'},
            'body' : 'great day today'
        }, 
        {
            'author': {'username': 'Bob'},
            'body': 'great day.'
        }
    ]
    return render_template('index.html', title='Home', user=user, posts=posts)

# create the login page
@app.route('/login', methods=['GET', 'POST'])
def login():
    form = LoginForm()
    if form.validate_on_submit():
        flash("Login requested for user {}, password: {}, remember_me={}".format(
            form.username.data, form.password.data, form.remember_me.data
        ))
        return redirect(url_for('index'))
    #     user = Users.query.filter_by(username = form.username.data).first() # query table to look up username which was submitted in the form and see if it exists
    #     if user:
    #         # check the hash
    #         if check_password_hash(user.password_hash, form.password.data): # compare what's already in the database, what the user just typed into the form
    #             login_user(user) # log them in
    #             flash("Login successful")
    #             return redirect(url_for('dashboard'))
    #         else:
    #             flash("Wrong password, try again")
    #     else:
    #         flash("That user does not exist")

    return render_template('login.html', title='Log In', form=form) # template with the name 'form'='form' object created above


# # add CKEditor
# ckeditor = CKEditor(app)

# # add database
# app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///tradelens.db' #(now in config.py)

# # secret key
# app.config['SECRET_KEY'] = "e07b43t" #(now in config.py)

# UPLOAD_FOLDER = 'static/images'
# app.config['UPLOAD_FOLDER'] = UPLOAD_FOLDER

# # Initialize the database with the Flask app
# db.init_app(app) #(now in __init__.py) 
# # migrate database
# migrate = Migrate(app, db) #(now in __init__.py)

# # Flask_Login
# login_manager = LoginManager()
# login_manager.init_app(app)
# login_manager.login_view = 'login'

# @login_manager.user_loader
# def load_user(user_id):
#     return Users.query.get(int(user_id))

# # pass things to navbar
# @app.context_processor
# def base():
#     form = SearchForm() 
#     #the search form will get passed into the base.html which then passes to the navbar since it includes the navbar.html
#     return dict(form=form)

# # create admin page
# @app.route('/admin')
# @login_required
# def admin():
#     id = current_user.id
#     if id == 1:
#         return render_template("admin.html")
#     else:
#         flash("You are not an admin")
#         return redirect(url_for('dashboard'))

# # create search function
# @app.route('/search', methods=['POST'])
# def search():
#     form = SearchForm()
#     posts = Posts.query
#     if form.validate_on_submit():
#         # get data from the submitted search form
#         post.searched = form.searched.data
#         # query the database
#         posts = posts.filter(Posts.content.like('%' + post.searched + "%")) # filter by the content, doesn't have to be exact match
#         posts = posts.order_by(Posts.title).all() # return the results by title
#         return render_template("search.html", form=form, searched=post.searched, posts=posts)


# # log out
# @app.route('/logout', methods=['GET', 'POST'])
# @login_required
# def logout():
#     logout_user()
#     flash("You have been logged out")
#     return redirect(url_for('login'))

    
# # create initial dashboard page
# @app.route('/dashboard', methods=['GET', 'POST'])
# @login_required
# def dashboard():
#     form = user_form()
#     id = current_user.id
#     name_to_update = Users.query.get_or_404(id) # query the Users table, get or if it doesnt exists, pass in the id, which comes in from the <int: id> url

#     if request.method == 'POST':
#         name_to_update.name = request.form['name']
#         name_to_update.username = request.form['username']
#         name_to_update.email = request.form['email']
#         name_to_update.favorite_stock = request.form['favorite_stock']
#         name_to_update.about_author = request.form['about_author']

#         profile_pic = request.files['profile_pic'] # upload a file
#         # check for profile pic
#         if profile_pic:
#             try:
#                 # grab image name
#                 pic_filename = secure_filename(profile_pic.filename)
#                 # set uuid, since more than one person can have a pic with same file name
#                 pic_name = str(uuid.uuid1()) + "_" + pic_filename
#                 profile_pic.save(os.path.join(app.config['UPLOAD_FOLDER'], pic_name))
#                 # change to string to save to database
#                 name_to_update.profile_pic = pic_name
#             except Exception as e:
#                 flash('Error uploading profile picture: {}'.format(str(e)))
#                 return render_template("dashboard.html", form=form, name_to_update=name_to_update)

#         try:
#             db.session.commit()
#             flash('User updated successfully')
#             return render_template("dashboard.html", form=form, name_to_update=name_to_update)
#         except Exception as e:
#             db.session.rollback()
#             flash('Could not update user: {}'.format(str(e)))
#             return render_template("dashboard.html", form=form, name_to_update=name_to_update)
#     else:
#         return render_template("dashboard.html", form=form, name_to_update=name_to_update)

# # delete a post
# @app.route('/posts/delete/<int:id>')
# @login_required
# def delete_post(id):
#     post_to_delete = Posts.query.get_or_404(id)
#     if current_user.id == post_to_delete.poster.id:
#         try: 
#             db.session.delete(post_to_delete)
#             db.session.commit()
#             flash("Post has been deleted")
#             posts = Posts.query.order_by(Posts.date_posted)
#             return render_template("posts.html", posts=posts)
#         except:
#             # return error message
#             flash("Could not delete post")
#             return render_template("posts.html", posts=posts)
#     else:
#         flash("You cannot delete that post")
#         posts = Posts.query.order_by(Posts.date_posted)
#         return render_template("posts.html", posts=posts) 


# @app.route('/posts') #this will potentially become home page content
# def posts():
#     # grab all posts from the database
#     posts = Posts.query.order_by(Posts.date_posted) # query by chronological order, from the Posts model.
#     return render_template("posts.html", posts = posts)

# @app.route('/posts/<int:id>')
# def post(id):
#     post = Posts.query.get_or_404(id)
#     return render_template("post.html", post = post)

# @app.route('/posts/edit/<int:id>', methods=['GET', 'POST'])
# @login_required
# def edit_post(id):
#     post = Posts.query.get_or_404(id)
#     form = PostForm()
#     if form.validate_on_submit():
#         # Update the post attributes with the new data once edit submission is validated
#         post.title = form.title.data
#         # post.author = form.author.data
#         post.slug = form.slug.data
#         post.content = form.content.data
#         # update database with modifications
#         db.session.add(post)
#         db.session.commit()
#         flash("Post has been updated")
#         return redirect(url_for('post', id=post.id)) # redirect back to singular post page
    
#     if current_user.id == post.poster_id: # if id of logged in user matches the id of the poster of particular post
#         # Populate the form fields with current values of the post
#         form.title.data = post.title
#         # form.author.data = post.author
#         form.slug.data = post.slug
#         form.content.data = post.content
#         return render_template("edit_post.html", form=form) # goes back to newly edited singular post page
#     else:
#         flash("You cannot edit this post")
#         posts = Posts.query.order_by(Posts.date_posted)
#         return render_template("posts.html", posts=posts)

# # add posts page
# @app.route('/add_post', methods=['GET', 'POST'])
# # @login_required
# def add_post():
#     form = PostForm()
#     if form.validate_on_submit():
#         poster = current_user.id
#         post = Posts(title=form.title.data, content=form.content.data, poster_id=poster, slug=form.slug.data)
#         # clear form
#         form.title.data = ''
#         form.content.data = ''
#         # form.author.data = ''
#         form.slug.data = ''
#         #add post to database
#         db.session.add(post)
#         db.session.commit()

#         flash("Your post has been submitted")

#     return render_template("add_post.html", form=form)


# # webpage to return JSON (jsonify)
# @app.route('/date')
# def get_current_date():
#     favorite_companies = {
#         "Alex": "SoFi Techs",
#         "Chris": "Paypal Hold",
#         "Tim": "Affirm"
#     }
#     return favorite_companies
#     #return {"Date": date.today()}

# # delete a database record
# @app.route('/delete/<int:id>')
# def delete(id):
#     if id == current_user.id:
#         user_to_delete = Users.query.get_or_404(id)
#         name = None
#         form = user_form()
#         try:
#             db.session.delete(user_to_delete)
#             db.session.commit()
#             flash("User deleted successfully")
#             our_users = Users.query.order_by(Users.date_added)
#             return render_template("register.html", form=form, name=name, our_users=our_users)
#         except:
#             flash("Could not delete user")
#             return render_template("register.html", form=form, name=name, our_users=our_users)
#     else:
#         flash("You cannot delete that user")
#         return redirect(url_for('dashboard'))


# # update a database record
# @app.route('/update/<int:id>', methods=['GET', 'POST'])
# @login_required
# def update(id): 
#     form = user_form()
#     name_to_update = Users.query.get_or_404(id) # query the Users table, get or if it doesnt exists, pass in the id, which comes in from the <int: id> url
#     if request.method == 'POST':
#         name_to_update.name = request.form['name']
#         name_to_update.username = request.form['username']
#         name_to_update.email = request.form['email']
#         name_to_update.favorite_stock = request.form['favorite_stock']
#         try:
#             db.session.commit()
#             flash('User updated successfully')
#             return render_template("update.html", form=form, name_to_update=name_to_update, id=id)
#         except:
#             flash('Could not update user')
#             return render_template("update.html", form=form, name_to_update=name_to_update, id=id)
#     else:
#         return render_template("update.html", form=form, name_to_update=name_to_update, id=id)

# # add a user to database
# @app.route('/user/register', methods=['GET', 'POST'])
# def register():
#     name = None
#     form = user_form()
#     if form.validate_on_submit():
#         user = Users.query.filter_by(email=form.email.data).first() # this should return None, since all email addresses are unique
#         if user is None:
#             # Hash the password
#             hashed_pw = generate_password_hash(form.password_hash.data)
#             user = Users(name = form.name.data, username = form.username.data, email = form.email.data, favorite_stock = form.favorite_stock.data, password_hash=hashed_pw )
#             db.session.add(user)
#             db.session.commit()
#         name = form.name.data
#         form.name.data = ''
#         form.username.data = ''
#         form.email.data = ''
#         form.favorite_stock.data = ''
#         form.password_hash.data = ''
#         flash('Your account has been created')
#     our_users = Users.query.order_by(Users.date_added) # chronological order of account created
#     return render_template("register.html", form=form, name=name, our_users=our_users)

# @app.route('/user/<name>')
# def user(name):
#     return render_template("user.html", name=name)

# # custom error pages

# # invalid URL
# @app.errorhandler(404)
# def page_not_found(e):
#     return render_template("404.html")

# #internal server error
# @app.errorhandler(500)
# def page_not_found(e):
#     return render_template("500.html")

# # Create password test page
# @app.route('/test_pw', methods=['GET', 'POST'])
# def test_pw():
#     email = None
#     password = None
#     pw_to_check = None
#     passed = None
#     form = password_form()
#     # Validate form
#     if form.validate_on_submit():
#         email = form.email.data # assign to whatever user inputs
#         password = form.password_hash.data

#         form.email.data = '' # clear for next user
#         form.password_hash.data = ''

#         #lookup user by email address
#         pw_to_check = Users.query.filter_by(email=email).first()

#         #check hash pw
#         passed = check_password_hash(pw_to_check.password_hash, password)
#     return render_template("test_pw.html", email=email, password=password, pw_to_check=pw_to_check, passed=passed, form=form)

# # Create name page
# @app.route('/name', methods=['GET', 'POST'])
# def name():
#     name = None
#     form = name_form()
#     # Validate form
#     if form.validate_on_submit():
#         name = form.name.data # assign to whatever user inputs
#         form.name.data = '' # clear for next user
#         flash("Form submitted successfully")
#     return render_template("name.html", name=name, form=form)


# # display information for each company in the stocks table
# @app.route('/symbol/<symbol>')
# def symbol(symbol):
#     stock = Stocks.query.filter_by(ticker_symbol=symbol).first()
#     if stock:
#         return render_template("symbol.html", stock=stock)
#     else:
#         return render_template("404.html")


# # if __name__ == '__main__':
# #     app.run(debug=True)