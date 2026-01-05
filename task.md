## Workflow to retrieve/parse/use SEC filings and data

#### Complex procedure with many potential steps, choices lie for orchestrating this process:

- Web-based Admin dashboard
- CLI that references Clients/Services in other layers

The following assumes the latter.

I do predict it's unlikely that there will be a single source of truth for company metric data whether it's from one of 
the financial statements, a KPI from the MD&A section, from the investor presentation, or a derived metric.

#### Data retrieval

Options for getting data from financial statements:
- iXBRL parsing (machine-readable)
- Third party APIs
- use LLM

In the iXBRL docs, there are no tags for metrics from the MD&A section. There needs to be more manual processing and domain
knowledge as it relates to what is important about the company.

Some metrics are nowhere in the 10Q's and 10K's, but are in the Investor Presentation/Supplemental Materials, e.g.
Palantir (PLTR) 'Billings', or Uber (UBER) 'Monthly Trips per Monthly Active Platform Consumer'.
Finnhub offers a premium endpoint that returns presentations/slides usually used during earnings calls in PDF format:

```shell
curl "https://finnhub.io/api/v1/stock/presentation?symbol=IBM&token="
```

#### General process

___

## Subscriptions and payments integration with user auth flows

#### Stripe webhooks + claims + 
