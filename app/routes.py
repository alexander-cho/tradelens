from datetime import datetime, timezone

from flask import render_template, flash, redirect, url_for, request
from flask_login import login_user, current_user, logout_user, login_required

import sqlalchemy as sa

from app import app, db
from app.forms import LoginForm, RegistrationForm, EditProfileForm, EmptyForm, PostForm, SearchForm
from app.models import User, Post, Stocks

from scripts.get_yf_ohlcv import get_ohlcv, get_shares_outstanding
from scripts.large_holders import get_institutional_holders, get_insider_transactions
from scripts.earnings_ipos import get_ipos_data, get_earnings_calendar, IPO_URL, EARNINGS_URL
from scripts.general_info import get_fast_info, get_calendar
from scripts.options import get_expiry_list, get_options_detail, get_option_chain_for_expiry


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


# front page
@app.route('/')
@app.route('/index')
# @login_required
def index():
    return render_template('index.html', title='Home')


# login
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


# logout
@app.route('/logout')
@login_required
def logout():
    logout_user()
    flash("You have been logged out")
    return redirect(url_for('index'))


# register an account
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


# create a post
@app.route('/add-post', methods=['GET', 'POST'])
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
            return redirect(url_for('feed'))
        else:
            # clear form
            form.title.data = ''
            form.content.data = ''
            flash("That stock does not exist or is not in the database yet")
            return redirect(url_for('add_post'))
    return render_template('add_post.html', form=form, stock=stock)


# read a specific post
@app.route('/post/<int:id>')
def post(id):
    post = Post.query.get_or_404(id)
    return render_template("post.html", post=post)


# update/edit a post
@app.route('/post/<int:id>/edit', methods=['GET', 'POST'])
@login_required
def edit_post(id):
    post = Post.query.get_or_404(id)
    form = PostForm()
    if form.validate_on_submit():
        # Update the post attributes with the new data once edit submission is validated
        post.title = form.title.data
        # post.author = form.author.data
        # post.slug = form.slug.data
        post.content = form.content.data
        # update database with modifications
        db.session.add(post)
        db.session.commit()
        flash("Post has been updated")
        return redirect(url_for('post', id=post.id))  # redirect back to singular post page

    if current_user.id == post.author.id:  # if id of logged-in user matches the id of the author of particular post
        # Populate the form fields with current values of the post
        form.title.data = post.title
        # form.author.data = post.author
        # form.slug.data = post.slug
        form.content.data = post.content
        return render_template("edit_post.html", form=form)  # goes back to newly edited singular post page
    else:
        flash("You cannot edit this post")
        posts = Post.query.order_by(Post.date_posted)
        return render_template("feed.html", posts=posts)


# delete a post
@app.route('/post/<int:id>/delete')
@login_required
def delete_post(id):
    post_to_delete = Post.query.get_or_404(id)
    if current_user.id == post_to_delete.author.id:
        try:
            db.session.delete(post_to_delete)
            db.session.commit()
            flash("Post has been deleted")
            posts = Post.query.order_by(Post.timestamp.desc())
            return render_template("feed.html", posts=posts)
        except:
            # return error message
            flash("Could not delete post")
            posts = Post.query.order_by(Post.timestamp.desc())
            return render_template("feed.html", posts=posts)
    else:
        flash("You cannot delete that post")
        posts = Post.query.order_by(Post.timestamp.desc())
        return render_template("feed.html", posts=posts)


# show the whole post feed
@app.route('/feed')
def feed():
    # grab all posts from the database
    posts = Post.query.order_by(Post.timestamp.desc())  # query by chronological order, from the Posts model.
    return render_template('feed.html', posts=posts)


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


# symbol directory route
@app.route('/symbol') 
def symbol_main(): 
    stock_list = Stocks.query.all() 
    return render_template('symbol_main.html', title='Symbol Directory', stock_list=stock_list)


# search for a company
@app.route('/symbol-search', methods=['POST'])
def symbol_search():
    form = SearchForm()
    if form.validate_on_submit():
        search_content = form.searched.data.upper()
        searched_ticker_found = Stocks.query.filter_by(ticker_symbol=search_content).first()
        if searched_ticker_found:
            return redirect(url_for('symbol', symbol=search_content))
        else:
            flash("That stock does not exist or is not in the database yet")
            return redirect(url_for('symbol_main'))
    else:
        return redirect(url_for('symbol_main'))


# display information for each company in the stocks table
@app.route('/symbol/<symbol>', methods=['GET', 'POST'])
def symbol(symbol):
    stock = db.session.scalar(sa.select(Stocks).where(Stocks.ticker_symbol == symbol))
    symbol_posts = Post.query.order_by(Post.timestamp.desc()).where(Post.title == symbol)

    ohlcv_data = get_ohlcv(symbol)
    institutional_holders = get_institutional_holders(symbol)
    insider_transactions = get_insider_transactions(symbol)
    shares_outstanding = get_shares_outstanding(symbol)
    fast_info = get_fast_info(symbol)
    calendar = get_calendar(symbol)

    return render_template('symbol.html', title=f'{stock.company_name} ({stock.ticker_symbol})', stock=stock, symbol_posts=symbol_posts, ohlcv_data=ohlcv_data, institutional_holders=institutional_holders, insider_transactions=insider_transactions, shares_outstanding=shares_outstanding, fast_info=fast_info, calendar=calendar)

    # form = PostForm()  # functionality for submitting post directly on specific symbol page
    # if request.method == 'POST' and form.validate_on_submit:
    #     if form.title.data == symbol:  # if the ticker that user inputs in the field matches current symbol page
    #         post = Post(title=form.title.data, content=form.content.data, timestamp=datetime.now(timezone.utc), user_id=current_user.id)
    #         db.session.add(post)
    #         db.session.commit()
    #         flash("Your idea has been submitted")
    #         return redirect(url_for('symbol', symbol=form.title.data))
    #     elif form.title.data != symbol:  # if they don't match
    #         stock_exists = Stocks.query.filter(Stocks.ticker_symbol == form.title.data).first()  # check if the user entered exists in the DB
    #         if stock_exists:
    #             flash(f"That was the page for {symbol}, you cannot post that there. Here you go:")
    #             return redirect(url_for('symbol', symbol=form.title.data))  # send them to the symbol page they entered
    #         else:
    #             flash("That stock does not exist or is not in the database yet")
    #             return redirect(url_for('index'))
    # if stock:
    #     if tutes_data:  # if the institutional info is not null in the database
    #         tutes = json.loads(tutes_data)  # turn valid json into python list of dictionaries
    #         return render_template('symbol.html', title=f'{stock.company_name} ({stock.ticker_symbol})', stock=stock, posts=posts, form=form, tutes=tutes)
    #     else:
    #         return render_template('symbol.html', title=f'{stock.company_name} ({stock.ticker_symbol})', stock=stock, posts=posts, form=form)  # without tute data
    # else:
    #     return render_template('404.html')


# return IPO's anticipated in the next 3 months
@app.route('/earnings-ipos')
def earnings_ipos():
    ipo_data = get_ipos_data(IPO_URL)
    earnings_calendar = get_earnings_calendar()
    return render_template('earnings_ipos.html', title='IPOs', ipo_data=ipo_data, earnings_calendar=earnings_calendar)


@app.route('/options')
def options_main():
    pass


@app.route('/options/<symbol>')
def options(symbol):
    stock = db.session.scalar(sa.select(Stocks).where(Stocks.ticker_symbol == symbol))
    expiry_list = get_expiry_list(symbol=stock.ticker_symbol)
    options_detail = get_options_detail(symbol=stock.ticker_symbol)
    return render_template('options.html', title='Options', stock=stock, expiry_list=expiry_list, options_detail=options_detail)


@app.route('/options/<symbol>/<expiry_date>')
def options_expiry(symbol, expiry_date):
    stock = db.session.scalar(sa.select(Stocks).where(Stocks.ticker_symbol == symbol))
    option_chain = get_option_chain_for_expiry(symbol, expiry_date)
    return render_template('options_expiry.html', title=f'{symbol} {expiry_date}', stock=stock, option_chain=option_chain, expiry_date=expiry_date)


# # if __name__ == '__main__':
# #     app.run(debug=True)
