# set the python version as a build-time argument with Python 3.12 as the default
ARG PYTHON_VERSION=3.12-slim-bullseye
FROM python:${PYTHON_VERSION}

# create a virtual environment
RUN python -m venv /opt/venv

# set the virtual environment as the current location
ENV PATH=/opt/venv/bin:$PATH

# upgrade pip
RUN pip install --upgrade pip

# set Python-related environment variables
ENV PYTHONDONTWRITEBYTECODE=1
ENV PYTHONUNBUFFERED=1

# Copy the requirements file and install dependencies
COPY requirements.txt /requirements.txt
RUN pip install -r /requirements.txt

# Copy the entire src directory to the container
COPY src /src

# Set the working directory to /src
WORKDIR /src

# Set Flask app environment variable
ENV FLASK_APP=tradelens.py

# Expose port 5000
EXPOSE 5000

# Set the entrypoint to the boot.sh script with an absolute path
ENTRYPOINT ["/boot.sh"]
