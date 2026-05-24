<div style="text-align:center">
    <img src="logo/tl-logo.png" width="140" height="140" alt=""/>
</div>
<br>
<div style="text-align:center">
    <img src="https://custom-icon-badges.demolab.com/badge/No%20AI-2f2f2f?logo=non-ai&logoColor=white" alt=""/>
    <img src="https://img.shields.io/badge/.NET-10-purple" alt=""/>
    <img src="https://img.shields.io/badge/Angular-20-red" alt=""/>
    <img src="https://img.shields.io/badge/Postgres-17-blue" alt=""/>
    <img src="https://img.shields.io/badge/Firefox-FF7139?logo=firefoxbrowser&logoColor=white)" alt=""/>
</div>

---

### All of your financial market data and investing needs in one place, for you to be very profitable in the long run. Zoom in on the information that actually matters.

#### Future implementation ideas

Check out `task.md` or `docs/`. Or submit a feature request: `.github/ISSUE_TEMPLATE/feature-request.yml`

___

#### Run everything locally with Docker

From the repository root run:

```shell
cd docker && docker compose -p tradelens up -d
```

**Note**: For this to work, create and populate `appsettings.Staging.json` under `tradelens/src/Tradelens.Api/`,
using the`tradelens/src/Tradelens.Api/appsettings.Example.json` as the template. Contact me for test keys, URLs,
and/or environment variables.

___

### Repository structure

The main platform lies under `tradelens/`, while any user-facing applications and the like are inside `apps/`.

Api: controllers, app entry point, helpers/logic related to request/response cycle

Core: domain and business logic

Infrastructure: data access concerns like databases, external APIs, file I/O, etc.

Cli: another client, to orchestrate document fetching/parsing among other concerns → reference/call related services in
inner layers

Worker: background services and workers, e.g. run DB refresh on a schedule, etc.

___

![UBER Dashboard](https://github.com/user-attachments/assets/7d0ebd40-234c-461b-a775-434828532ed5)

___

#### Ideas and suggestions from v1

cache certain API responses to save API calls and faster response times (time and money)

- AlphaVantage Top Gainers/Losers/MostActivelyTraded are updated once a day.
- for the time being, stock/options contract charts since we are limited to a measly 5 per minute.

better UI for charts - think of client side rendering maybe

___

#### Postgres local dev container

```shell
docker run -d \
--name tradelens-db-dev \
-e POSTGRES_USER=postgres \
-e POSTGRES_PASSWORD=postgres \
-e POSTGRES_DB=tradelens-db-dev \
-p 5434:5432 \
postgres:17.2-alpine3.21
```

#### Redis local dev container

```shell
docker run -d \
--name tradelens-redis-dev \
-p 6380:6379 \
redis:latest
```

___

#### EF Core Migrations (from inside `tradelens/src/`)

```shell
dotnet ef migrations add <Migration Name> -s Tradelens.Api -p Tradelens.Infrastructure
dotnet ef database update -s Tradelens.Api -p Tradelens.Infrastructure
```

`-s` flag: --startup-project; specify executable project to run (Program.cs)

`-p` flag: --project; specify since migration files, DbContext lie in a separate class library

___

#### .gitattributes

```terminaloutput
warning: in the working copy of '.github/workflows/deploy_azure.yml', CRLF will be replaced by LF the next time Git touches it
```
