from datetime import datetime, timezone

from flask import render_template, flash, redirect, url_for, request
from flask_login import login_user, current_user, logout_user, login_required

import sqlalchemy as sa

from app import app, db
from app.forms import LoginForm, RegistrationForm, EditProfileForm, EmptyForm, PostForm, SearchForm
from app.models import User, Post, Stocks

from src.providers.yfinance_ import YFinance
from src.providers.alphavantage import AlphaVantage
from src.providers.finnhub import Finnhub
from src.providers.federalreserve import FederalReserve
from src.providers.tradier import Tradier

from src.utils.date_ranges import get_date_range_ahead, get_date_range_past


@app.before_request
def before_request():
    # if current_user is logged in
    if current_user.is_authenticated:
        # set last seen field to current time
        current_user.last_seen = datetime.now(timezone.utc)
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
    finnhub = Finnhub()
    alphavantage = AlphaVantage()

    market_status = finnhub.get_market_status(),
    top_gainers_losers = alphavantage.get_top_gainers_losers()

    return render_template('index.html',
                           title='Home',
                           market_status=market_status,
                           top_gainers_losers=top_gainers_losers)


# login
@app.route('/login', methods=['GET', 'POST'])
def login():
    if current_user.is_authenticated:
        # return to index page if already logged in
        return redirect(url_for('index'))

    form = LoginForm()
    if form.validate_on_submit():
        # query table to look up username which was submitted in the form and see if it exists
        user = User.query.filter_by(username=form.username.data).first()
        # if the user exists
        if user:
            # check the hash
            # compare what's already in the database with what the user just typed into the form
            if user.check_password(form.password.data):
                # log them in
                login_user(user, remember=form.remember_me.data)
                flash("Login successful")
                return redirect(url_for('index')) 
            else:
                flash("Wrong password, try again")
                return redirect(url_for('login'))
        else:
            flash("That user does not exist")

    return render_template('login.html',
                           title='Log In',
                           form=form)


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
        # return to index page if already logged in
        return redirect(url_for('index'))

    form = RegistrationForm()
    if form.validate_on_submit():
        user = User(username=form.username.data,
                    email=form.email.data,
                    date_joined=datetime.now(timezone.utc))
        user.set_password(form.password_hash.data)
        db.session.add(user)
        db.session.commit()
        flash("Your account has been created")
        return redirect(url_for('login'))

    return render_template('register.html',
                           title='Register',
                           form=form)


# user profile
@app.route('/user/<username>')
@login_required
def user(username):
    user = User.query.filter_by(username=username).first_or_404()
    form = EmptyForm()
    posts = Post.query.order_by(Post.timestamp.desc()).where(Post.user_id == user.id)

    return render_template('user.html',
                           title=f'{user.username}',
                           user=user,
                           posts=posts,
                           form=form)


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
        return redirect(url_for('user',
                                username=current_user.username))
    elif request.method == 'GET':
        form.username.data = current_user.username
        form.about_me.data = current_user.about_me

    return render_template('edit_profile.html',
                           title='Edit Profile',
                           form=form)
        

# follow a user
@app.route('/follow/<username>', methods=['POST'])
@login_required
def follow(username):
    form = EmptyForm()
    if form.validate_on_submit():
        # query user to follow
        user = db.session.scalar(sa.select(User).where(User.username == username))
        # if they don't exist in the database
        if user is None:
            flash("User not found")
            return redirect(url_for('index'))
        # if it is yourself
        if user == current_user:
            flash("You can't follow yourself")
            return redirect(url_for('user',
                                    username=username))
        current_user.follow(user)
        db.session.commit()
        flash(f"You are now following {username}")
        return redirect(url_for('user',
                                username=username))
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
            return redirect(url_for('user',
                                    username=username))
        current_user.unfollow(user)
        db.session.commit()
        flash(f"You have unfollowed {username}")
        return redirect(url_for('user',
                                username=username))
    else:
        return redirect(url_for('index'))


# create a post
@app.route('/add-post', methods=['POST'])
@login_required
def add_post():
    form = PostForm()
    if request.method == 'POST' and form.validate_on_submit():
        if form.title.data.upper() == symbol:
            post = Post(title=form.title.data.upper(),
                        content=form.content.data,
                        timestamp=datetime.now(timezone.utc),
                        user_id=current_user.id)
            db.session.add(post)
            db.session.commit()
            flash("Your idea has been submitted")
            return redirect(url_for('symbol',
                                    symbol=form.title.data.upper()))
        else:
            flash("Wrong symbol, you cannot post that here.")
            return redirect(url_for('symbol', symbol=form.title.data.upper()))
    else:
        flash("Invalid request")
        return redirect(url_for('index'))


# read a specific post
@app.route('/post/<int:id>')
def post(id):
    post = Post.query.get_or_404(id)

    return render_template("post.html",
                           post=post)


# update/edit a post
@app.route('/post/<int:id>/edit', methods=['GET', 'POST'])
@login_required
def edit_post(id):
    post = Post.query.get_or_404(id)
    form = PostForm()

    if form.validate_on_submit():
        # Update the post attributes with the new data once edit submission is validated
        post.title = form.title.data.upper()
        # post.author = form.author.data
        # post.slug = form.slug.data
        post.content = form.content.data
        # update database with modifications
        db.session.add(post)
        db.session.commit()
        flash("Post has been updated")
        # redirect back to singular post page
        return redirect(url_for('post',
                                id=post.id))

    # if id of logged-in user matches the id of the author of particular post
    if current_user.id == post.author.id:
        # Populate the form fields with current values of the post
        form.title.data = post.title
        # form.author.data = post.author
        # form.slug.data = post.slug
        form.content.data = post.content
        # goes back to newly edited singular post page
        return render_template("edit_post.html",
                               form=form)
    else:
        flash("You cannot edit this post")
        posts = Post.query.order_by(Post.date_posted)
        return render_template("feed.html",
                               posts=posts)


# delete a post
@app.route('/post/<int:id>/delete', methods=['GET', 'POST'])
@login_required
def delete_post(id):
    post_to_delete = Post.query.get_or_404(id)
    if current_user.id == post_to_delete.author.id:
        try:
            db.session.delete(post_to_delete)
            db.session.commit()
            flash("Post has been deleted")
            return redirect(url_for('feed'))
        except:
            # return error message
            flash("Could not delete post")
            posts = Post.query.order_by(Post.timestamp.desc())
            return render_template("feed.html",
                                   posts=posts)
    else:
        flash("You cannot delete that post")
        posts = Post.query.order_by(Post.timestamp.desc())
        return render_template("feed.html",
                               posts=posts)


# show the whole post feed
@app.route('/feed')
def feed():
    # grab all posts from the database, query by chronological order from the Posts model.
    posts = Post.query.order_by(Post.timestamp.desc())
    return render_template('feed.html',
                           posts=posts)


# search for post content
@app.route('/search', methods=['POST'])
def search():
    form = SearchForm()
    if form.validate_on_submit():
        # get data from submitted form
        search_content = form.searched.data
        posts = Post.query.filter(Post.content.like('%' + search_content + '%'))
        display_posts = posts.order_by(Post.title).all()
        return render_template('search.html',
                               form=form,
                               searched=search_content,
                               display_posts=display_posts)
    # if invalid or blank search is submitted
    else:
        return redirect(url_for('index'))


# symbol directory route
@app.route('/symbol')
def symbol_main(): 
    stock_list = Stocks.query.all() 
    return render_template('symbol_main.html',
                           title='Symbol Directory',
                           stock_list=stock_list)


# search for a company
@app.route('/symbol-search', methods=['POST'])
def symbol_search():
    form = SearchForm()
    if form.validate_on_submit():
        search_content = form.searched.data.upper()
        searched_ticker_found = Stocks.query.filter_by(ticker_symbol=search_content).first()
        if searched_ticker_found:
            return redirect(url_for('symbol',
                                    symbol=search_content))
        else:
            flash("That stock does not exist or is not in our database yet")
            return redirect(url_for('symbol_main'))
    else:
        return redirect(url_for('symbol_main'))


# display information for each company in the stocks table
@app.route('/symbol/<symbol>', methods=['GET', 'POST'])
def symbol(symbol):
    # if user manually enters the ticker in lowercase letters in the url
    symbol = symbol.upper()
    if request.path != f"/symbol/{symbol}":
        return redirect(url_for('symbol',
                                symbol=symbol))

    # query the stock table to retrieve the corresponding symbol
    stock = db.session.query(Stocks).filter(Stocks.ticker_symbol == symbol).first()
    # query the posts associated with the symbol
    symbol_posts = Post.query.order_by(Post.timestamp.desc()).where(Post.title == symbol)

    # CONTEXT FROM SRC FOR SYMBOL DATA
    yfinance = YFinance(symbol)
    finnhub = Finnhub()

    ohlcv_data = yfinance.get_day_ohlcv()
    basic_info = yfinance.get_info()
    main_info = yfinance.get_underlying_for_price_info()
    fast_info = yfinance.get_fast_info()
    calendar = yfinance.get_calendar()
    institutional_holders = yfinance.get_institutional_holders()
    insider_transactions = yfinance.get_insider_transactions()
    analyst_ratings = yfinance.get_analyst_ratings()
    company_profile = finnhub.get_company_profile(ticker=symbol)

    (past_date, today) = get_date_range_past(days_past=365)
    insider_sentiment = finnhub.get_insider_sentiment(ticker=symbol, _from=past_date, to=today)
    lobbying_activities = finnhub.get_lobbying_activities(ticker=symbol, _from=past_date, to=today)
    government_spending = finnhub.get_government_spending(ticker=symbol, _from=past_date, to=today)

    # # ADDING A POST ON THE SYMBOL PAGE
    form = PostForm()
    if form.validate_on_submit():
        add_post()

    return render_template('symbol.html',
                           title=f'{stock.company_name} ({stock.ticker_symbol})',
                           stock=stock,
                           form=form,
                           symbol_posts=symbol_posts,
                           ohlcv_data=ohlcv_data,
                           basic_info=basic_info,
                           main_info=main_info,
                           fast_info=fast_info,
                           calendar=calendar,
                           institutional_holders=institutional_holders,
                           insider_transactions=insider_transactions,
                           analyst_ratings=analyst_ratings,
                           company_profile=company_profile,
                           insider_sentiment=insider_sentiment,
                           lobbying_activities=lobbying_activities,
                           government_spending=government_spending)


# return IPOs anticipated in the next 3 months, upcoming earnings calendar
@app.route('/earnings-ipos', methods=['GET'])
def earnings_ipos():
    alphavantage = AlphaVantage()
    finnhub = Finnhub()

    ipo_data = alphavantage.get_ipos_data()

    (today, future_date) = get_date_range_ahead(days_ahead=14)
    earnings_calendar = finnhub.get_earnings_calendar(_from=today, to=future_date)

    return render_template('earnings_ipos.html',
                           title='IPOs',
                           ipo_data=ipo_data,
                           earnings_calendar=earnings_calendar)


@app.route('/options/<symbol>')
def options(symbol):
    stock = db.session.scalar(sa.select(Stocks).where(Stocks.ticker_symbol == symbol))

    yfinance = YFinance(symbol)
    expiry_list = yfinance.get_expiry_list()

    return render_template('options.html',
                           title=f"{symbol} - Options Expiration Calendar",
                           stock=stock,
                           expiry_list=expiry_list)


@app.route('/options/<symbol>/<expiry_date>')
def options_expiry(symbol, expiry_date):
    stock = db.session.scalar(sa.select(Stocks).where(Stocks.ticker_symbol == symbol))

    yfinance = YFinance(symbol)
    tradier = Tradier()

    open_interest = yfinance.get_open_interest(expiry_date)
    volume = yfinance.get_volume(expiry_date)
    implied_volatility = yfinance.get_implied_volatility(expiry_date)
    last_bid_ask = yfinance.get_last_price_bid_ask(expiry_date)

    tradier_options = tradier.get_options_chain(symbol, expiry_date)

    return render_template('options_expiry.html',
                           title=f'{symbol} {expiry_date}',
                           stock=stock,
                           expiry_date=expiry_date,
                           open_interest=open_interest,
                           volume=volume,
                           implied_volatility=implied_volatility,
                           last_bid_ask=last_bid_ask,
                           tradier_options=tradier_options)


@app.route('/symbol/<symbol>/news')
def symbol_news(symbol):
    stock = db.session.query(Stocks).filter(Stocks.ticker_symbol == symbol).first()

    finnhub = Finnhub()

    (past_date, today) = get_date_range_past(days_past=7)
    ticker_news = finnhub.get_stock_news(ticker=stock.ticker_symbol, _from=past_date, to=today)

    return render_template('symbol_news.html',
                           title=f'News for {symbol}',
                           stock=stock,
                           ticker_news=ticker_news)


@app.route('/macro')
def macro():
    federal_reserve = FederalReserve()

    gdp = federal_reserve.get_quarterly_gdp()
    cpi = federal_reserve.get_cpi()

    return render_template('macro.html',
                           title='Macro',
                           gdp=gdp,
                           cpi=cpi)


@app.route('/technical-screener')
def technical_screener():
    return render_template('technical_screener.html',
                           title='Technical Screener')


@app.route('/market-news', methods=['GET'])
def market_news():
    if request.method == 'GET':
        finnhub = Finnhub()
        news = finnhub.get_market_news()

        return render_template('market_news.html',
                               title='News',
                               news=news)


# if __name__ == '__main__':
#     app.run(debug=True)
