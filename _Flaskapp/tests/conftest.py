import pytest
from app import create_app, db
from config import TestConfig


@pytest.fixture()
def app():
    app = create_app(config_class=TestConfig)

    with app.app_context():
        db.create_all()

    yield app


@pytest.fixture()
def client(app):
    return app.test_client()
