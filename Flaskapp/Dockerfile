# python
FROM python:3.12-slim-bullseye

# install os dependencies
RUN apt-get update && apt-get install -y \
    # for postgres
    libpq-dev \
    gcc

# set the working directory
WORKDIR /app

# copy the requirements file and install dependencies
COPY requirements.txt requirements.txt
RUN pip install -r requirements.txt

# copy the application code, migrations, static, and configuration
COPY app app
COPY migrations migrations
COPY modules modules
COPY static static
COPY resources resources

COPY config.py tradelens.py ./

# copy environmental variables for local docker deployment
COPY .flaskenv .env ./

# copy the script to populate the stocks table
COPY modules/db/tickers_to_db.py ./

EXPOSE 5000

# entrypoint to run database migrations, populate data, and start the application
CMD ["sh", "-c", "flask db upgrade && python tickers_to_db.py && flask run --host=0.0.0.0"]
