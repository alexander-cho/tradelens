# Closely Monitor Your System

This app in a development environment currently runs in local containers: the client application inside the wwwroot
directory in the backend web api, and two supporting services- a postgres database and a redis cache. Getting insight
into what is happening within these "black boxes" could be of great use, for example, I could see which endpoints 
users access the most and put more development focus towards the features that support that. I would also be able to
identify any bottlenecks as it relates to the user experience, quickly seeing which method returns what error message
in order to take the optimal action. A structured logging system that helps me see exactly what, when, and where 
something happens would assist me immensely in terms of development, as well as strategy and marketing.

### Components

**Promtail** is an agent that discovers targets, or systems to be monitored, attaches a label to their local log stream,
then pushes the logs to a processor or aggregator instance such as **Loki**. Then we can use a tool like **Grafana** to query 
and visualize almost everything about these logs and more.

### Procedure

Our app and backing services are already up and running locally, as I've defined in a docker-compose file inside the 
backend/ directory.

`cd backend`

Make sure docker is working and running properly.

```bash
$ docker -v
```
```
Docker version 28.5.1, build e180ab8
```

Run the following commands to create three directories to store configuration for each of the mentioned technologies
as well as any other related information. Creating an 'observability' directory or similar is obviously optional, but be
sure to make note of your folder structure.

`mkdir observability`
`cd observability`
`mkdir promtail`
`mkdir loki`
`mkdir grafana`

First, set up the instructions to spin up a container running Loki.

```yml
  loki:
    container_name: loki
    image: grafana/loki:3.5
    platform: linux/arm64
    volumes:
      - ./observability/loki/loki-config.yml:/mnt/config/loki-config.yml
    ports:
      - "3100:3100"
    command: -config.file=/mnt/config/loki-config.yml
```

The image to be pulled is the latest version, with arm64 as I am on a M1 Mac. Look at the registry here to see
what is compatible with your machine architecture: https://hub.docker.com/r/grafana/loki/tags
Then create a volume for it. This config file can be anywhere on your local machine, but we will put it inside the
directory we defined earlier. Then we map it to the path *inside* the container that runs it so Loki can access
it from within. Then expose ports, here 3100 on the left side, the host machine's port to access Loki externally from
either the web, Grafana, etc. 3100 as well on the right side, which is where the Loki image is pre-configured to listen
on by default. Define the command to be run when the container is started, which will use our specific configurations.

Now set up Promtail.

```yml
  promtail:
    container_name: promtail
    image: grafana/promtail:3.4.2-arm64
    ports:
      - "9080:9080"
    volumes:
      - ./observability/promtail/promtail-config.yml:/mnt/config/promtail-config.yml
#      - /var/log:/var/log
      - /var/lib/docker/containers:/var/lib/docker/containers
      - /var/run/docker.sock:/var/run/docker.sock
    depends_on:
      - loki
    command: -config.file=/mnt/config/promtail-config.yml
#    environment:
#      - LOKI_URL=http://localhost:3100/loki/api/v1/push
```

Here we create a few volumes. Mount the local Promtail config file to the path in docker container.
`/var/log:/var/log` allows Promtail running inside the container to access the logs on our local machine. I've
commented it out for now.
`/var/lib/docker/containers` is where Docker stores container logs, and by mounting this, Promtail can access logs
for individual containers running in the local system.
`/var/run/docker.sock` is the Unix domain socket which will allow Promtail to interact with the Docker daemon and
discover running containers. Now two process on the same computer (Docker and Promtail) can communicate with each other
by using this file system path as their endpoint.

Finally, set up Grafana.

```yml
  grafana:
    container_name: grafana
    image: grafana/grafana:11.5.2
    user: "501"
    platform: linux/arm64
    depends_on:
      - loki
    ports:
      - "3000:3000"
    environment:
      - GF_SECURITY_ADMIN_USER=user
      - GF_SECURITY_ADMIN_PASSWORD=password
    volumes:
      - ./observability/grafana/grafana.ini:/etc/grafana/grafana.ini
```

Specify the image. I set the user to "501", after running the command `id` in the terminal, it should show up as uid. 
This gives the Grafana permission to write to the mounted config file.

#### Configurations

Now define the configurations for each service, for Loki, you can find the recommended structure in `./observability/loki/loki-config.yml`.

For Promtail configuration at `./observability/promtail/promtail-config.yml`, note the `clients` key value `http://loki:3100/loki/api/v1/push`.
This is the endpoint where the logs will be pushed to. Also note we use the name `loki` directly since it's what is defined
in the docker compose file within the same network. Once again, I've commented out the instructions for scraping the
local machine logs. To scrape from Docker, we set the `docker_sd_configs:` host name to use Docker's service discovery by connecting
to the socket, to dynamically search for containers and their logs without having to specify a file path. We can also manipulate
the labels of the scraped logs, with `relabel_configs:`; `source_labels:` to extract container name from Docker's metadata,
and set `target_label: container` as we will use this identifier later on.

There's one more thing we need to consider in order for Loki to discover these logs from Docker, and that's to install the 
Grafana Loki Docker Driver Client, which is a plugin to enable this, and adding useful labels to our log groups so we can
query them easily in Grafana as we will see later on, and many more. It can be done with this command (you need to perform this
on every Docker host which you want to collect logs from.) Read more about it here: `https://grafana.com/docs/loki/latest/send-data/docker-driver/configuration/`

```bash
$ docker plugin install grafana/loki-docker-driver:3.3.2-arm64 --alias loki --grant-all-permissions
```

I've added the `-arm64` to the image tag, which you may have to modify.

Check to make sure it is installed:

```bash
$ docker plugin ls
```

```
ID             NAME          DESCRIPTION           ENABLED
6b6eb7fc65da   loki:latest   Loki Logging Driver   true
```

Now, as per the config instructions from the Grafana docs, linked above, we need to configure the Loki logging driver as the default
for all containers. Locate `~/.docker/daemon.json` in the local system and update with the following.

```json
{
  "debug": true,
  "log-driver": "loki",
  "log-opts": {
    "loki-batch-size": "400",
    "loki-url": "http://localhost:3100/loki/api/v1/push"
  }
}
```

Once commiting changes to this file, **restart** the Docker daemon for it to take effect. All newly created containers will send logs
to Loki via the driver.

Now we run everything, make sure we're in the `backend/` directory, run

```bash
$ docker compose up -d --force-recreate
```

Have all the containers started? Check by running:
```bash
$ docker ps
```
```
CONTAINER ID   IMAGE                          COMMAND                  CREATED          STATUS          PORTS                                         NAMES
95b98ebf3a63   grafana/promtail:3.4.2-arm64   "/usr/bin/promtail -…"   47 seconds ago   Up 46 seconds   0.0.0.0:9080->9080/tcp, [::]:9080->9080/tcp   promtail
0bc6efcffa9b   grafana/grafana:11.5.2         "/run.sh"                47 seconds ago   Up 46 seconds   0.0.0.0:3000->3000/tcp, [::]:3000->3000/tcp   grafana
a251248f28d0   backend-app                    "dotnet API.dll"         47 seconds ago   Up 46 seconds   0.0.0.0:6502->6501/tcp, [::]:6502->6501/tcp   tradelens-app
3a71e41d4016   grafana/loki:3.5               "/usr/bin/loki -conf…"   48 seconds ago   Up 46 seconds   0.0.0.0:3100->3100/tcp, [::]:3100->3100/tcp   loki
6af3dde0977d   redis:latest                   "docker-entrypoint.s…"   48 seconds ago   Up 46 seconds   0.0.0.0:6379->6379/tcp, [::]:6379->6379/tcp   redis
c7e2fb5ab34b   postgres:17.2-alpine3.21       "docker-entrypoint.s…"   48 seconds ago   Up 46 seconds   0.0.0.0:5432->5432/tcp, [::]:5432->5432/tcp   tradelens-db
```
There it is.

How do we know things are working though? Go to `http://localhost:3100/metrics`. There should be a ton of different metrics displayed.
Head over to `http://localhost:3100/ready` should say 'ready'. If it doesn't and says something like `Ingester not ready: waiting for 15s after being ready`,
then wait for a few moments and then just refresh the page. We're ready to ingest some logs.

Now head over to Grafana, which we configured to `http://localhost:3000`. Log in- the default username and password
is `admin`, but as we specified otherwise in the docker compose, we will authenticate with those credentials.

In order to query the logs ingested by Loki, set it as a data source inside of Grafana. Select Add Data Source

![Grafana add data source](https://github.com/user-attachments/assets/c429a148-6a1f-431a-9731-3f231e9d9b2f)

Then select Loki. Now enter the url under 'Connection'. The input placeholder may say `http://localhost:3100`, but
if you remember our promtail config url definition, use `http://loki:3100`, then scroll down and click 'Save & test'.

![Add Loki](images/https://github.com/user-attachments/assets/13e5dcf5-76c9-4d23-bfe8-e897354b3a80)

Once that is successful, to query the data, click **Explore** on the side menu, then make sure Loki is set as the default
data source, which it should be. Here we build the LogQL command, click the 'Select label' dropdown menu, and you will see 
quite a few labels, thanks to the Docker driver plugin that was installed earlier:

![Loki labels](https://github.com/user-attachments/assets/f200ecfd-4ef5-4c42-8ce1-67007a9bf005)

For the 'Select value' dropdown, you should be able to see all the containers running in your local system!

![Loki labels](https://github.com/user-attachments/assets/3149a1b2-c24d-401c-b10e-8f95768cbbc9)

The dropdown only shows containers which have logs for that selected time period so make sure to modify that:

![Loki labels](https://github.com/user-attachments/assets/bcdd7f57-aecb-444b-b62a-6081b459d661)

If i want to filter all log records relating to dotnet inside my application container, I can use the following query:

`{container_name="tradelens-app"} |= "dotnet"`

![Grafana Logs Drilldown](https://github.com/user-attachments/assets/3101ecb5-1f20-4027-93fc-02ee247fe5b7)
