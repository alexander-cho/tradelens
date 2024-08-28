FROM python:3.12-slim-bullseye

# install os dependencies
RUN apt-get update && apt-get install -y \
    # for postgres
    libpq-dev \
    gcc

# Set the working directory
WORKDIR /app

# Copy the requirements file and install dependencies
COPY requirements.txt requirements.txt
RUN pip install -r requirements.txt

# Copy the application code, migrations, static, and configuration
COPY app app
COPY migrations migrations
COPY modules modules
COPY static static

COPY config.py tradelens.py ./

# copy as well the .flaskenv and .env for regular docker deployment
# COPY .flaskenv .env ./

# Run database migrations and populate the stocks table
# Use a script to perform these actions in the container
COPY tickers_to_db.py ./

# Expose the port Flask will run on
EXPOSE 5000

# Set up the entrypoint to run database migrations, populate data, and start the application
CMD ["sh", "-c", "flask db upgrade && flask run --host=0.0.0.0"]
