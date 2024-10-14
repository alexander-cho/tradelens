from datetime import datetime, timezone

from flask import render_template, redirect, url_for
from flask_login import current_user

from app import db

from .forms import SearchForm

from ..models import Post

from modules.providers.alphavantage import AlphaVantage
from modules.providers.finnhub_ import Finnhub

from . import bp_main


@bp_main.before_request
def before_request():
    # if current_user is logged in
    if current_user.is_authenticated:
        # set last seen field to current time
        current_user.last_seen = datetime.now(timezone.utc)
        db.session.commit()


# front page
@bp_main.route('/')
@bp_main.route('/index')
def index():
    finnhub = Finnhub()
    alphavantage = AlphaVantage()

    market_status = finnhub.get_market_status()
    top_gainers_losers = alphavantage.get_top_gainers_losers()
    market_holidays = finnhub.get_market_holidays()

    return render_template(
        template_name_or_list='main/index.html',
        title='Home',
        market_status=market_status,
        top_gainers_losers=top_gainers_losers,
        market_holidays=market_holidays
    )


# search for post content
@bp_main.route('/search', methods=['POST'])
def search():
    form = SearchForm()
    if form.validate_on_submit():
        # get data from submitted form
        search_content = form.searched.data
        posts = Post.query.filter(Post.content.like('%' + search_content + '%'))
        display_posts = posts.order_by(Post.title).all()
        return render_template(
            template_name_or_list='main/search.html',
            form=form,
            searched=search_content,
            display_posts=display_posts
        )
    # if invalid or blank search is submitted
    else:
        return redirect(url_for('main.index'))
