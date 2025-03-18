from datetime import datetime, timezone

from flask import render_template, flash, redirect, url_for, request
from flask_login import current_user, login_required, login_user, logout_user

from .forms import LoginForm, RegistrationForm

from app.models import User

from app import db

from . import bp_auth


# login
@bp_auth.route('/login', methods=['GET', 'POST'])
def login():
    if current_user.is_authenticated:
        # return to index page if already logged in
        return redirect(url_for('main.index'))

    login_form = LoginForm()
    if login_form.validate_on_submit():
        # query table to look up username which was submitted in the form and see if it exists
        user = User.query.filter_by(username=login_form.username.data).first()
        # if the user exists
        if user:
            # check the hash, compare what's already in the database with what the user just typed into the form
            if user.check_password(login_form.password.data):
                login_user(user, remember=login_form.remember_me.data)
                flash("Login successful")
                return redirect(url_for('main.index'))
            else:
                flash("Wrong password, try again")
                return redirect(url_for('auth.login'))
        else:
            flash("That user does not exist")

    return render_template(
        template_name_or_list='auth/login.html',
        title='Log In',
        login_form=login_form
    )


# logout
@bp_auth.route('/logout', methods=['GET'])
@login_required
def logout():
    if request.method == "GET":
        logout_user()
        flash("You have been logged out")
        return redirect(url_for('main.index'))
    else:
        flash("Invalid request")
        return redirect(url_for('main.index'))


# register
@bp_auth.route('/register', methods=['GET', 'POST'])
def register():
    if current_user.is_authenticated:
        # return to index page if already logged in
        return redirect(url_for('main.index'))

    registration_form = RegistrationForm()
    if registration_form.validate_on_submit():
        user = User(
            username=registration_form.username.data,
            email=registration_form.email.data,
            date_joined=datetime.now(timezone.utc)
        )
        user.set_password(registration_form.password_hash.data)
        db.session.add(user)
        db.session.commit()
        flash("Your account has been created")
        return redirect(url_for('auth.login'))

    return render_template(
        template_name_or_list='register.html',
        title='Register',
        registration_form=registration_form
    )
