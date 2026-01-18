# Investing is Best Served Visually

Blocking out the noise and focusing on what really matters when it comes to investing in good public companies can be 
difficult but extremely rewarding financially. Now, what really matters? Is it the fact that the CTO sold 2,745 shares
last month to fund who knows what, a new boat? Is it when an analyst at a big bank reiterates a bearish price 
target the morning after the company reports blowout quarterly earnings and raises guidance? Or could it be that some 
guy on YouTube with laser eyes in his profile picture says the stock just formed a *double bearish hammerhead quad 
witching shooting star doji with bearish MACD divergence in Mercury retrograde*™ and the stock is going to zero? 
Nope. It's... *drumroll*... Gross Profits. Revenue growth. Various ratios. Margins by segment. Company specific KPI's. 
Maybe it's not exactly what you wanted to hear, but it's what will make you rich in the long term, and individual investors 
like you and I should aim to maximize time inside the markets.

They say that a picture paints a thousand words. That's why I embarked on the journey of translating dense, lengthy, and 
quite frankly boring financial statements into beautiful visualizations that are easy on the eyes. There's no better way
to tell a convincing story with data, in my honest opinion.

### Fast Prototype

My initial python prototype (not in this repository, but you can find it [here](https://tradelens-py-327dfc1283f7.herokuapp.com/symbol/AAPL/financials)
using the Flask web microframework along with Jinja2 templating was fairly quick to spin up. I sourced data from the 
Yahoo Finance API and instantiated line charts in the html using [Plotly](https://plotly.com/python/). I had a good time 
manipulating pandas dataframes and figuring out the shape of the responses. Now about the data, it's not comprehensive 
nor is it the most effective... but it *is* there for starters.

![TradeLens Flask AAPL Financials](https://github.com/user-attachments/assets/9faab1f0-cb7a-4780-ac43-fb61365046f1)

The charts are fully rendered server-side but lacks client-side interactivity, and the lines for each metric are plastered 
on a single chart all at once. Some recurring pieces of feedback that I received included but were not limited to "Too plain"
or"I want to see gross and operating margins for the non-auto segments, side by side". I realized I needed more interactivity,
as well as fundamental data that digs much deeper into the business.

### Looking for a more robust solution

For my production app, I found [Financial Modeling Prep](https://site.financialmodelingprep.com/developer/docs#income-statement)
(FMP), which along with numerous other endpoints, their income statement API gave me a wide array of important financial
metrics returned in a neat json structure. Using the configured HttpClientFactory as a service in my app container, and some DTO
mapping, I was able to get the response for either yearly or quarterly metrics (period) of a specific company (ticker symbol).
`backend/src/Infrastructure/Clients/Fmp/FmpClient.cs`.

I feel like part of a great user experience lies in giving them flexibility and choice, so I knew I needed functionality
for the option to get the specific metrics they want, for whichever period in less than a few clicks. The best way to 
achieve this is to pass the metrics as a list in the url parameters as something like: 
`GET /api/companies?ticker=SOFI&period=quarter&metric=netIncome&metric=revenue&metric=freeCashFlow`
This is defined in the `GetFundamentalData` method in the corresponding controller class:
`backend/src/API/Controllers/CompaniesController.cs`

https://github.com/user-attachments/assets/68b7cd7b-397f-49a7-a1c4-03070607a343

Now for the interesting part: My implementation of the orchestration for this API in the service layer was unorthodox, and
frankly not optimal nor scalable when I look back at it. You can see it at 
`backend/src/Infrastructure/Services/CompanyFundamentalsService.cs`.
Here, I am getting nine different metrics across three FMP endpoints: income statement, balance sheet, cash flow statement.
The metrics `revenue` and `net income` for example both come from the income statement endpoint, so if a user requests for
those together, I need a way to call the endpoint just once. The `GetCompanyFundamentalMetricsAsync` method from the file
mentioned above achieves this with deduplication. I initialize an empty list `methodsToCall`, and then loop through each
requested metric, against a pre-defined mapping as seen in `backend/src/Core/Constants/CompanyFundamentalMetricMappings.cs`
and check if the method I need to call (aforementioned three FMP endpoints) exists in `methodsToCall`. If not, add it.
Then I create nullable `Task` objects for each of the methods that are needed based on `methodsToCall`, then add the
non-null tasks to a list and execute them in parallel using `Task.WhenAll()`. After all tasks complete, I unwrap the 
results from each completed task into their respective domain model variables (IncomeStatement, BalanceSheet, CashFlowStatement)
Now I loop through each requested metric again, determine which dataset it belongs to using the mapping, then extract 
just that specific field from the appropriate domain model. Then transform it into a standardized `Metric` object 
containing the metric name and a list of `ValueDataAtEachPeriod` (with Period, FiscalYear, and Value). Finally, I aggregate
the response by collecting all the transformed metrics into a single `CompanyFundamentalsResponse` to return.

That was quite a bit. If you actually look through the method (viewer discretion advised), you will see a lot of repetition
in the form of nested switch statements, first by endpoint type then by specific metric.

These metrics do not change nor do they get updates super often. Could that mean that populating a database schema with 
these values is potentially more scalable and efficient? Initial thoughts tell me that this would require a hefty amount
of backfilling for already reported quarters and years, for a vast amount of companies, for a wide variety of metrics.
Should I write a script that makes periodic calls (sleep/schedule to not hit API rate limits) to the FMP endpoints,
i.e.:

```
for each ticker in database[0:50]
    for each metric in a pre-defined list (or just every single one FMP response provides)
        populate db table according to defined schema
```

Each and every individual metric value becomes its own database row with its unique attributes.

### A big problem

One of my favorite publicly traded companies (one I hold shares/options in, comprising a very large part of my portfolio) 
is SoFi Technologies, Inc., a fintech pursuing what might be the most vertically integrated vision in modern consumer banking. 
They're simultaneously operating as a bank, a technology infrastructure provider, and a consumer fintech app—a trifecta 
that would seem insane if they weren't actually pulling it off. With management executing relentlessly on their vision of 
a true "one-stop shop for all your financial needs," SoFi has become a force that established players can no longer ignore.

To even begin illustrating this, let's take a look at their stunning quarterly revenues, the most basic of metrics.

![SoFi Quarterly Revenue](https://github.com/user-attachments/assets/455920bb-586e-4f7d-9507-ad37b3df50cb)

Amazing. But wait a minute. These values are just outright wrong. SoFi did \$961.6 million in the third quarter of 2025. The
current data shows that the past 4 quarters they passed the \$1 billion mark, which would be great as my position would be
worth more, but this is incorrect data.

Data accuracy and integrity are persistent challenges in the financial industry, with numerous documented cases of
erroneous reporting from aggregation services. While I would consider accepting minor discrepancies in less critical
metrics, finding such a significant error in basic revenue data—and specifically for the company representing my
largest holding left me with some reservations about FMP.

This presents an interesting challenge where rather than relying on a single third party API, I decided to undertake the
monumental task of obtaining the necessary data on my own directly from sources like the official SEC filings.

I do understand how this kind of error could occur. Financial services companies have arguably the most complex SEC
filings of any sector due to their heavily regulated nature. Between GAAP and non-GAAP metrics, fair value measurements,
loan loss provisions, net interest margin calculations, these filings are labyrinths. 
To put this in perspective: SoFi's latest 10-Q spans 121 pages, compared to just 42 for Airbnb and a mere 29 for Apple. 
When an API is scraping hundreds of tickers across diverse sectors, it's not surprising that edge cases in complex 
filings lead to errors, especially with similar-sounding line items.

Let's examine this mishap to the core. Upon looking at the latest 10-Q filing (Q3 2024) for SoFi Technologies, and
performing `command+F` for the term `1,268`, just one instance appears
[here (page 35)](https://d18rn0p25nwr6d.cloudfront.net/CIK-0001818874/0ba872f4-4717-4519-aac6-c8d21355d789.pdf)
in the form of "Interest Rate Swaps": "Nine months ended, September 30, 2024", under
"Note 10: Derivative Financial Instruments", as (1,268). That can't be it. Let's head over to the official company facts
JSON [file](https://data.sec.gov/api/xbrl/companyfacts/CIK0001818874.json)
which is an utterly comprehensive collection of metrics that are US-GAAP compliant provided by the SEC Edgar API,
and do the same search. The four instances that begin with "1268" are under the *annual* filings for `Accounts Payable`
in FY 2022 and `Research and Development Expenses` in Q2 2023.

This is worse than I initially thought. FMP is either pulling dollar values incorrectly or computing random ones. In the
Q3 2025 10-Q pdf file, searching for `961.6` reveals 8 instances of the correct revenue amount, all under the proper
contexts, e.g. Consolidated Statements of Operations, management discussion sections, and year-over-year comparison tables.
This solidified my decision: I would build my own solution to programmatically parse SEC filings using LLM-based extraction,
while understanding document structure and financial context.

### The solution

This involves quite a few steps so meticulous planning with room for some mistakes along the way would be helpful.
At a high level, I would build a console application (CLI) separate from the API and class libraries under `/src`,
to download the desired filings to my local system. Then I would have to build a service in the `Infrastructure` class lib
with the LLM parsing functionality, according to business rules declared in `Core`, e.g. which metrics to look for depending
on the company.

I'll try to explain this from the top down to figure out what is really needed. There will be 9 or 16 initial metric
charts displayed once the user navigates to a company dashboard. There will be a button to reset it back to that initial
setting, to switch between quarterly and annual readings, and importantly the functionality to select the metrics they
want.

In the 'Edit Charts' select metrics modal, if there are many (dozens) of possible metrics to display, I'd like to have
multiple dropdowns for several different types of distinctions. This would require me to have a database column of
`sectionFromFiling`: income statement, balance sheet, cash flow, company specific, etc.

I will have metrics like "Revenue" be single axis, so in other words one bar per x value. But for a metric like
"Operating Expenses", I would like to have three bars stacked making the distinction between "Sales and Marketing",
"General and Administrative", and "Research and Development". This would mean that I make each of these a separate database
row, and have another field along the lines of `parentMetric`, and have it null for "Revenue", for example.

__Ticker__|__Period__|__Year__|__Metric__|__ParentMetric__|__Value__|__SectionFromFiling__|__SourcedFrom__|__PeriodEndDate__|__Unit__
__SOFI__|__Q3__|__2025__|__Revenue__|__null__|__961,600,000__|__Consolidated Statement of Operations__|__10-Q__|__2025-09-30__|__Dollars__
__SOFI__|__Q3__|__2025__|__Research and Development__|__Operating Expenses__|__961,600,000__|__Consolidated Statement of Operations__|__10-Q__|__2025-09-30__|__Dollars__

LlmExtractionService in `Infrastructure` to parse files and populate DB schema

FinancialMetricRepo in `Infrastructure` for data access

In Companies Controller, use this for tickers that have data populated, or else fallback to FMP data

