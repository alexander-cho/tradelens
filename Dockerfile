FROM python:3.12-slim-bullseye

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
COPY resources resources
COPY config.py .flaskenv tradelens.py ./

# Run database migrations and populate the stocks table
# Use a script to perform these actions in the container
COPY tickers_to_db.py ./

# Expose the port Flask will run on
EXPOSE 5000

# Set up the entrypoint to run database migrations, populate data, and start the application
CMD ["sh", "-c", "flask db upgrade && python tickers_to_db.py && flask run --host=0.0.0.0"]
