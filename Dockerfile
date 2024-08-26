FROM python:3.12-slim-bullseye

# Set the working directory to where app.py is located
WORKDIR /src/app

# Copy the requirements file and install dependencies
COPY requirements.txt /src/
RUN pip install --no-cache-dir -r /src/requirements.txt

# Copy the entire src directory to the container
COPY src /src

# Set the FLASK_APP environment variable to the filename without the .py extension
ENV FLASK_APP=app

# Expose the port Flask will run on
EXPOSE 5000

# Run the Flask application
CMD ["flask", "run", "--host=0.0.0.0"]
