from flask import render_template, flash, redirect, url_for, request
from flask_login import current_user, login_required

import sqlalchemy as sa

from app import db

from .forms import EditProfileForm, EmptyForm

from ..models import User, Post

from . import bp_users


# user profile
@bp_users.route('/user/<username>')
@login_required
def user(username):
    user = User.query.filter_by(username=username).first_or_404()
    form = EmptyForm()
    posts = Post.query.order_by(Post.timestamp.desc()).where(Post.user_id == user.id)

    return render_template(
        template_name_or_list='users/user.html',
        title=f'{user.username}',
        user=user,
        posts=posts,
        form=form
    )


# edit profile
@bp_users.route('/edit_profile', methods=['GET', 'POST'])
@login_required
def edit_profile():
    form = EditProfileForm(current_user.username)

    if form.validate_on_submit():
        current_user.username = form.username.data
        current_user.about_me = form.about_me.data
        db.session.commit()
        flash("Your information has been updated")
        return redirect(url_for('users.user', username=current_user.username))
    elif request.method == 'GET':
        form.username.data = current_user.username
        form.about_me.data = current_user.about_me

    return render_template(
        template_name_or_list='users/edit_profile.html',
        title='Edit Profile',
        form=form
    )


# follow a user
@bp_users.route('/follow/<username>', methods=['POST'])
@login_required
def follow(username):
    form = EmptyForm()
    if form.validate_on_submit():
        # query user to follow
        user = db.session.scalar(sa.select(User).where(User.username == username))
        # if they don't exist in the database
        if user is None:
            flash("User not found")
            return redirect(url_for('main.index'))
        # if it is yourself
        if user == current_user:
            flash("You can't follow yourself")
            return redirect(url_for('users.user', username=username))
        current_user.follow(user)
        db.session.commit()
        flash(f"You are now following {username}")
        return redirect(url_for('users.user', username=username))
    else:
        return redirect(url_for('main.index'))


# unfollow a user
@bp_users.route('/unfollow/<username>', methods=['POST'])
@login_required
def unfollow(username):
    form = EmptyForm()
    if form.validate_on_submit():
        user = db.session.scalar(sa.select(User).where(User.username == username))
        if user is None:
            flash("User not found")
            return redirect(url_for('main.index'))
        if user == current_user:
            flash("You can't unfollow yourself")
            return redirect(url_for('users.user', username=username))
        current_user.unfollow(user)
        db.session.commit()
        flash(f"You have unfollowed {username}")
        return redirect(url_for('users.user', username=username))
    else:
        return redirect(url_for('main.index'))
