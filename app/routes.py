import json
# import os
# import uuid as uuid
from datetime import datetime, timezone

from flask import render_template, flash, redirect, url_for, request
from flask_login import login_user, current_user, logout_user, login_required
# from flask_ckeditor import CKEditor
# from werkzeug.utils import secure_filename

import sqlalchemy as sa

from app import app, db
from app.forms import LoginForm, RegistrationForm, EditProfileForm, EmptyForm, PostForm, SearchForm
from app.models import User, Post, Stocks

from scripts.ipos import get_ipos_data
from scripts.options import get_call_options


@app.before_request
def before_request():
    if current_user.is_authenticated:  # if current_user is logged in
        current_user.last_seen = datetime.now(timezone.utc)  # set last seen field to current time
        db.session.commit()


# Pass search form to navbar
@app.context_processor
def base():
    form = SearchForm()
    return dict(form=form)


@app.route('/')
@app.route('/index')
# @login_required
def index():
    if current_user.is_authenticated:  # show following posts if logged in
        posts = db.session.scalars(current_user.following_posts()).all()
        return render_template('index.html', title='Home', posts=posts)
    else: # if not logged in show the entire feed
        posts = Post.query.order_by(Post.timestamp.desc())
        return render_template('index.html', title='Home', posts=posts)


# create the login page
@app.route('/login', methods=['GET', 'POST'])
def login():
    if current_user.is_authenticated:
        return redirect(url_for('index')) # return to index page if already logged in
    form = LoginForm()
    if form.validate_on_submit():
        user = User.query.filter_by(username = form.username.data).first()  # query table to look up username which was submitted in the form and see if it exists
        if user:  # if the user exists
            # check the hash
            if user.check_password(form.password.data):  # compare what's already in the database with what the user just typed into the form
                login_user(user, remember=form.remember_me.data)  # log them in
                flash("Login successful")
                return redirect(url_for('index')) 
            else:
                flash("Wrong password, try again")
                return redirect(url_for('login'))
        else:
            flash("That user does not exist")

    return render_template('login.html', title='Log In', form=form)  # template with the name 'form'='form' object created above


# log out
@app.route('/logout')  #, methods=['GET', 'POST'])
@login_required
def logout():
    logout_user()
    flash("You have been logged out")
    return redirect(url_for('index'))


# register a user
@app.route('/register', methods=['GET', 'POST'])
def register():
    if current_user.is_authenticated:
        return redirect(url_for('index'))  # return to index page if already logged in
    form = RegistrationForm()
    if form.validate_on_submit():
        user = User(username=form.username.data, email=form.email.data, date_joined=datetime.now(timezone.utc))
        user.set_password(form.password_hash.data)
        db.session.add(user)
        db.session.commit()
        flash("Your account has been created")
        return redirect(url_for('login'))
    return render_template('register.html', title='Register', form=form)


# user profile
@app.route('/user/<username>')
@login_required
def user(username):
    user = User.query.filter_by(username=username).first_or_404()
    form = EmptyForm()
    posts = Post.query.order_by(Post.timestamp).where(Post.user_id == user.id)
    return render_template('user.html', title=f'{user.username}', user=user, posts=posts, form=form)


# edit your profile
@app.route('/edit_profile', methods=['GET', 'POST'])
@login_required
def edit_profile():
    form = EditProfileForm(current_user.username)
    if form.validate_on_submit():
        current_user.username = form.username.data
        current_user.about_me = form.about_me.data
        db.session.commit()
        flash("Your information has been updated")
        return redirect(url_for('user', username=current_user.username))
    elif request.method == 'GET':
        form.username.data = current_user.username
        form.about_me.data = current_user.about_me
    return render_template('edit_profile.html', title='Edit Profile', form=form)
        

# follow a user
@app.route('/follow/<username>', methods=['POST'])
@login_required
def follow(username):
    form = EmptyForm()
    if form.validate_on_submit():
        user = db.session.scalar(sa.select(User).where(User.username == username))  # query user to follow
        if user is None:  # if they don't exist in the database
            flash("User not found")
            return redirect(url_for('index'))
        if user == current_user:  # if it is yourself
            flash("You can't follow yourself")
            return redirect(url_for('user', username=username))
        current_user.follow(user)
        db.session.commit()
        flash(f"You are now following {username}")
        return redirect(url_for('user', username=username))
    else:
        return redirect(url_for('index'))
    

# unfollow a user
@app.route('/unfollow/<username>', methods=['POST'])
@login_required
def unfollow(username):
    form = EmptyForm()
    if form.validate_on_submit():
        user = db.session.scalar(sa.select(User).where(User.username == username))
        if user is None:
            flash("User not found")
            return redirect(url_for('index'))
        if user == current_user:
            flash("You can't unfollow yourself")
            return redirect(url_for('user', username=username))
        current_user.unfollow(user)
        db.session.commit()
        flash(f"You have unfollowed {username}")
        return redirect(url_for('user', username=username))
    else:
        return redirect(url_for('index'))


# add posts page
@app.route('/add_post', methods=['GET', 'POST'])
# @login_required
def add_post():
    form = PostForm()
    stock = Stocks.query.filter(Stocks.ticker_symbol == form.title.data).first()  # check if the title (ticker) exists in the database
    if form.validate_on_submit():
        if stock:
            post = Post(title=form.title.data, content=form.content.data, timestamp=datetime.now(timezone.utc), user_id=current_user.id)
            db.session.add(post)
            db.session.commit()
            flash("Your idea has been submitted")
            return redirect(url_for('posts'))
        else:
            # clear form
            form.title.data = ''
            form.content.data = ''
            flash("That stock does not exist or is not in the database yet")
            return redirect(url_for('add_post'))
    return render_template('add_post.html', form=form, stock=stock)


@app.route('/posts')  # this will potentially become home page content
def posts():
    # grab all posts from the database
    posts = Post.query.order_by(Post.timestamp.desc())  # query by chronological order, from the Posts model.
    return render_template('posts.html', posts=posts)


# search for post content
@app.route('/search', methods=['POST'])
def search():
    form = SearchForm()
    if form.validate_on_submit():
        search_content = form.searched.data  # get data from submitted form
        posts = Post.query.filter(Post.content.like('%' + search_content + '%'))
        display_posts = posts.order_by(Post.title).all()
        return render_template('search.html', form=form, searched=search_content, display_posts=display_posts)
    else:  # if invalid or blank search is submitted
        return redirect(url_for('index'))
    

# search for a company
@app.route('/symbol-search', methods=['POST'])
def symbol_search():
    form = SearchForm()
    if form.validate_on_submit():
        search_content = form.searched.data
        stock = Stocks.query.filter(Stocks.ticker_symbol == search_content).first()
        return render_template('symbol_search.html', form=form, searched=search_content, stock=stock)
    else:
        return redirect(url_for('symbol_main'))


# # add CKEditor
# ckeditor = CKEditor(app)


# UPLOAD_FOLDER = 'static/images'
# app.config['UPLOAD_FOLDER'] = UPLOAD_FOLDER


# @login_manager.user_loader
# def load_user(user_id):
#     return Users.query.get(int(user_id))


    
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
# @login_required/
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
    
#     if current_user.id == post.poster_id: # if id of logged-in user matches the id of the poster of particular post
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


# # webpage to return JSON (jsonify)
# @app.route('/date')
# def get_current_date():
#     favorite_companies = {
#         "Alex": "SoFi Techs",
#         "Chris": "Paypal Hold",
#         "Tim": "Affirm"
#     }
#     return favorite_companies


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


# symbol directory route 
@app.route('/symbol') 
def symbol_main(): 
    stock_list = Stocks.query.all() 
    return render_template('symbol_main.html', title='Symbol Directory', stock_list=stock_list)


# display information for each company in the stocks table
@app.route('/symbol/<symbol>', methods=['GET', 'POST'])
def symbol(symbol):
    stock = db.session.scalar(sa.select(Stocks).where(Stocks.ticker_symbol == symbol))
    posts = Post.query.order_by(Post.timestamp.desc()).where(Post.title == symbol)
    tutes_data = db.session.scalar(sa.select(Stocks.institutional_info).where(Stocks.ticker_symbol == symbol))
    form = PostForm()  # functionality for submitting post directly on specific symbol page
    if request.method == 'POST' and form.validate_on_submit:
        if form.title.data == symbol:  # if the ticker that user inputs in the field matches current symbol page
            post = Post(title=form.title.data, content=form.content.data, timestamp=datetime.now(timezone.utc), user_id=current_user.id)
            db.session.add(post)
            db.session.commit()
            flash("Your idea has been submitted")
            return redirect(url_for('symbol', symbol=form.title.data))
        elif form.title.data != symbol:  # if they don't match
            stock_exists = Stocks.query.filter(Stocks.ticker_symbol == form.title.data).first()  # check if the user entered exists in the DB
            if stock_exists: 
                flash(f"That was the page for {symbol}, you cannot post that there. Here you go:")
                return redirect(url_for('symbol', symbol=form.title.data))  # send them to the symbol page they entered
            else:
                flash("That stock does not exist or is not in the database yet")
                return redirect(url_for('index'))
    if stock:
        if tutes_data:  # if the institutional info is not null in the database
            tutes = json.loads(tutes_data)  # turn valid json into python list of dictionaries
            return render_template('symbol.html', title=f'{stock.company_name} ({stock.ticker_symbol})', stock=stock, posts=posts, form=form, tutes=tutes)
        else:
            return render_template('symbol.html', title=f'{stock.company_name} ({stock.ticker_symbol})', stock=stock, posts=posts, form=form)  # without tute data
    else:
        return render_template('404.html')
    

# return IPO's anticipated in the next 3 months
@app.route('/ipos')
def ipos():
    csv_url = 'https://www.alphavantage.co/query?function=IPO_CALENDAR&apikey=GLLVZKDV4221RMO6'
    ipo_data = get_ipos_data(csv_url)
    return render_template('ipos.html', title='IPOs', ipo_data=ipo_data)


@app.route('/options')
def options():
    call_options = get_call_options()
    return render_template('options.html', call_options=call_options, title='Options')


# # if __name__ == '__main__':
# #     app.run(debug=True)
