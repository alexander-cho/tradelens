from flask import render_template

from app import app


@app.route('/', methods=['GET'])
def index():
    return render_template('index.html')


@app.route('/about')
def about() -> None:
    """
    Route for the user about page.

    Parameters:
        None

    Returns:
        None
    """
    name: str = 'Alex'
    age: int = 24
    about: list = ['student', 'sports']
    context: dict = {
        'name': name,
        'age': age,
        'about': about
    }
    return render_template('about.html', context=context)
