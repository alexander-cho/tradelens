from datetime import datetime, timezone

from flask import render_template, flash, redirect, url_for, request
from flask_login import current_user, login_required

from app import db
from .forms import PostForm, SearchForm
from ..models import Post

from . import bp_feed


# create a post
@bp_feed.route('/add-post', methods=['POST'])
@login_required
def add_post():
    form = PostForm()
    if request.method == 'POST' and form.validate_on_submit():
        post = Post(title=form.title.data.upper(),
                    content=form.content.data,
                    timestamp=datetime.now(timezone.utc),
                    user_id=current_user.id)
        db.session.add(post)
        db.session.commit()
        flash("Your idea has been submitted")
        return render_template('feed/add_post.html', form=form)
        # return redirect(url_for('symbol',
        #                         symbol=form.title.data.upper()))
    else:
        flash("Invalid request")
        return redirect(url_for('index'))


# read a specific post
@bp_feed.route('/post/<int:id>')
def post(id):
    post = Post.query.get_or_404(id)

    return render_template("feed/post.html",
                           post=post)


# update/edit a post
@bp_feed.route('/post/<int:id>/edit', methods=['GET', 'POST'])
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
        return render_template("feed/edit_post.html",
                               form=form)
    else:
        flash("You cannot edit this post")
        posts = Post.query.order_by(Post.date_posted)
        return render_template("feed/feed.html",
                               posts=posts)


# delete a post
@bp_feed.route('/post/<int:id>/delete', methods=['GET', 'POST'])
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
            return render_template("feed/feed.html",
                                   posts=posts)
    else:
        flash("You cannot delete that post")
        posts = Post.query.order_by(Post.timestamp.desc())
        return render_template("feed/feed.html",
                               posts=posts)


# show the whole post feed
@bp_feed.route('/feed')
def feed():
    # grab all posts from the database, query by chronological order from the Posts model.
    posts = Post.query.order_by(Post.timestamp.desc())
    return render_template('feed/feed.html',
                           posts=posts)


# search for post content
@bp_feed.route('/search', methods=['POST'])
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
