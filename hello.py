from flask import Flask, render_template

app = Flask(__name__)

@app.route('/')
def index():
    alex_name = "alex"
    favorite_stocks = ["SOFI", "LC", "AFRM", "UPST", 41]
    return render_template("index.html", alex_name=alex_name, favorite_stocks=favorite_stocks)

@app.route('/user/<name>')
def user(name):
    return render_template("user.html", name=name)

# custom error pages

#invalid URL
@app.errorhandler(404)
def page_not_found(e):
    return render_template("404.html")

#internal server error
@app.errorhandler(500)
def page_not_found(e):
    return render_template("500.html")







# if __name__ == '__main__':
#     app.run(debug=True)