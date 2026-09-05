# Orchestrating tasks away from the main app

### DB sync

2025 was a strong year for IPOs and capital raises in the US with nearly 350 companies ringing the opening bell. 
Another 376 got delisted, for various reasons including M&A activity and securities fraud. This tells us that the list of
companies we are working with tends to be dynamic, and it may look different even several days/weeks apart.

I am currently seeding the database upon app startup, specifically the `Stock` table using a manually stitched together 
JSON file with data that comes from this GitHub repo: `https://github.com/rreichel3/US-Stock-Symbols`. The three files 
we're concerned with are the following: 
`https://raw.githubusercontent.com/rreichel3/US-Stock-Symbols/refs/heads/main/nasdaq/nasdaq_full_tickers.json`,
`https://raw.githubusercontent.com/rreichel3/US-Stock-Symbols/refs/heads/main/amex/amex_full_tickers.json`, 
`https://raw.githubusercontent.com/rreichel3/US-Stock-Symbols/refs/heads/main/nyse/nyse_full_tickers.json`.

New companies go public and existing ones delist in a steady fashion, which means that our Stock table will serve more
and more stale data as time passes. We need a way to refresh the data regularly and do so accurately. Now we do have a
controller class with action methods in `Tradelens.Api` which allows us to execute commands via HTTP, but this may not
be the best choice for a number of reasons.

We can consider creating a background service that will periodically sync the Stock entities in our database with the 
proper/active company data from the GitHub repo. One main consideration is if this should be a part of the Api, our main
application, or in a separate project? Some potential problems arise, primarily that of resource sharing. The DB connection
that the background job utilizes to sync the DB rows would be the same as the ones the Api uses, and can slow down while
the task runs. We also want this process to be resilient and have its lifecycle decoupled from the main app. If the Api 
restarts or crashes, worker processes would be stopped mid-task or die completely.

The `Tradelens.Worker` project, which will contain our hosted services to run in the background will contain the logic
to orchestrate this inside individual Worker classes. We'll need to call the aforementioned GitHub links to retrieve the
data, and to fit it to our domain entity, which are Infrastructure and Core concerns, respectively. The Worker project
will reference those two projects.

Something else to worry about: How should we modify the existing data? There are a few ways we can go about this. We can
just drop the entire table, and fully re-seed with the new data. A problem with this is since the `Stock` and `CompanyMetric`
is a one-to-many relationship where the latter contains a foreign key that points to `Stock.Id`. If a Stock entity is 
removed, we would need a way to take care of the associated CompanyMetrics right there and then. Alternatively, we could
add a boolean `IsActive` field and a DateOnly `Checked` field to our `Stocks` table which we will initially set to `true` 
and today's date respectively. Then, we'll go through each object in the JSON, get its `symbol` attribute and query the 
DB for it. If it exists, record the `Checked` field, if not, add it as a new row. After going through the data, check 
which rows, if any, don't have the `Checked` field updated to today's date, we can presume that those are delisted; 
set `IsActive` to `false`. Determine what we should do with these "inactive" Stock entities, i.e. remove rows immediately,
remove after a set `TimeSpan`, etc. We should consider logging DB inserts, deletes, or field changes with the `ILogger<T>` 
abstraction to standard output or the OpenTelemetry collector.

How should we go about querying the `Stocks` table? Over-selecting a la `SELECT *` is famously inefficient for many cases
as heard in production scenarios, and may very well be the case for us too. Is it really the best idea to make these queries
for each `Stock` one by one using a foreach? We could prefer to make a query up front getting all the rows but retrieving
only the columns that are necessary, and make comparisons with an in memory list.
