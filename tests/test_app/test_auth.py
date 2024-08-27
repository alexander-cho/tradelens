import pytest

from app.models import User
from app import db


def test_login_title(client):
    """
    Test that the login route displays the correct title
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


@pytest.mark.parametrize("username, email, password", [
    ("user1", "user1@example.com", "password1"),
    ("user2", "user2@example.com", "password2"),
    ("user3", "user3@example.com", "password3"),
])
def test_logout(client, app, username, email, password):
    """
    GIVEN a user attempts to log out of their account
    WHEN a post request is sent from the user clicking logout button
    THEN the user should be logged out and redirected to the index page
    """
    with app.app_context():
        user = User(username=username, email=email)
        user.set_password(password)
        db.session.add(user)
        db.session.commit()

        # Log in the user
        client.post('/login', data={
            "username": username,
            "password": password,
        })

        # Log out the user
        response = client.get('/logout')

        # Check if the user is redirected to the index page upon logout
        assert response.status_code == 302


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
