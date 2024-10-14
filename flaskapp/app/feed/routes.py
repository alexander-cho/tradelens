from datetime import datetime, timezone

from flask import render_template, flash, redirect, url_for, request
from flask_login import current_user, login_required

from app import db
from .forms import PostForm
from ..models import Post

from . import bp_feed


# create a post
@bp_feed.route('/add-post', methods=['POST'])
@login_required
def add_post():
    post_form = PostForm()
    if request.method == 'POST' and post_form.validate_on_submit():
        post = Post(
            title=post_form.title.data.upper(),
            content=post_form.content.data,
            timestamp=datetime.now(timezone.utc),
            user_id=current_user.id
        )
        db.session.add(post)
        db.session.commit()
        flash("Your idea has been submitted")
        return redirect(url_for('stocks.symbol', symbol=post_form.title.data.upper()))
    else:
        flash("Invalid request")
        return redirect(url_for('main.index'))


# read a specific post
@bp_feed.route('/post/<int:id>')
def post(id):
    post = Post.query.get_or_404(id)

    return render_template(
        template_name_or_list="feed/post.html",
        post=post
    )


# update/edit a post
@bp_feed.route('/post/<int:id>/edit', methods=['GET', 'POST'])
@login_required
def edit_post(id):
    post = Post.query.get_or_404(id)
    post_form = PostForm()

    if post_form.validate_on_submit():
        # Update the post attributes with the new data once edit submission is validated, commit to db
        post.title = post_form.title.data.upper()
        post.content = post_form.content.data
        db.session.add(post)
        db.session.commit()
        flash("Post has been updated")
        # redirect back to singular post page
        return redirect(url_for('feed.post', id=post.id))

    # if id of logged-in user matches the id of the author of particular post
    if current_user.id == post.author.id:
        # Populate the form fields with current values of the post
        post_form.title.data = post.title
        post_form.content.data = post.content
        # goes back to newly edited singular post page
        return render_template(
            template_name_or_list="feed/edit_post.html",
            post_form=post_form
        )
    else:
        flash("You cannot edit this post")
        posts = Post.query.order_by(Post.date_posted)
        return render_template(
            template_name_or_list="feed/feed.html",
            posts=posts
        )


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
            return redirect(url_for('feed.feed'))
        except ValueError:
            # return error message
            flash("Could not delete post")
            posts = Post.query.order_by(Post.timestamp.desc())
            return render_template(
                template_name_or_list="feed/feed.html",
                posts=posts
            )
    else:
        flash("You cannot delete that post")
        posts = Post.query.order_by(Post.timestamp.desc())
        return render_template(
            template_name_or_list="feed/feed.html",
            posts=posts
        )


# show the whole post feed
@bp_feed.route('/feed')
def feed():
    # grab all posts from the database, query by chronological order from the Posts model.
    posts = Post.query.order_by(Post.timestamp.desc())
    return render_template(
        template_name_or_list='feed/feed.html',
        posts=posts
    )
