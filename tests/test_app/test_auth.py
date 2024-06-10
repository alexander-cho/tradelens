import pytest

from flask import url_for

from app.models import User
from app import db


def test_login_title(client):
    """
    Test that the login route has the correct title
    """
    response = client.get('/login')
    assert b"<title>Log In - TradeLens</title>" in response.data


@pytest.mark.parametrize("username, email, password", [
    ("alex", "alex.jhcho@gmail.com", "password123"),
    ("chris", "chris@gmail.com", "safe-password"),
    ("test", "test@gmail.com", "1234qwer!@#$"),
])
def test_login_success(client, app, username, email, password):
    """
    GIVEN a user logs in to their account with specified form data
    WHEN log in is attempted upon submission of the login form
    THEN the user should be redirected to the index page
    """
    # create users in the database to compare against
    with app.app_context():
        user = User(username=username, email=email)
        user.set_password(password)
        db.session.add(user)
        db.session.commit()

    response = client.post('/login', data={
        "username": username,
        "password": password,
    })

    # check if user is redirected to the index page upon successful login
    assert response.status_code == 302

    # Verify the user in the database
    with app.app_context():
        user = User.query.filter_by(username=username).first()
        assert user is not None
        assert user.check_password(password)


def test_logout(client):
    pass


@pytest.mark.parametrize("username, email, password", [
    ("alex", "alex.jhcho@gmail.com", "password123"),
    ("chris", "chris@gmail.com", "safe-password"),
    ("test", "test@gmail.com", "1234qwer!@#$"),
])
def test_registration(client, app, username, email, password):
    """
    GIVEN a user registers for an account with specified form data
    WHEN registration is attempted upon submission of the registration form
    THEN the user should be redirected to the login page
        AND user should be added to the database
        AND user's entered information should match the provided credentials
    """
    response = client.post('/register', data={
        "username": username,
        "email": email,
        "password_hash": password,
        "password_hash2": password
    })

    # check if user is redirected to the login page upon successful registration
    assert response.status_code == 302

    with app.app_context():
        # check if they are added to the database
        assert User.query.count() == 1

        # check if the credentials match
        user = User.query.first()
        assert user.username == username
        assert user.email == email
        assert user.check_password(password)
