# Rich command line utility

- get and parse SEC filings and investor presentations, provided with cli args. Mainly for backfilling.

### SEC filings and investor presentations

1. Common metrics from financial statements (Income, Balance Sheet, Cash Flow, Stockholders') will come from the EDGAR company facts API
2. KPI's (Management's Discussion and Analysis) will have to be parsed with LLM according to company-specific rules defined in Core
3. Data points from company presentations will have to take a back seat, there are paid options for this, i.e. `https://finnhub.io/docs/api/stock-presentation-slide-api`

- Any new filings (current reports, 8-K's) may have to be picked up via a background job and parsed similarly

Specify in command line prompt something to the tune of:

```shell
dotnet run && edgarfacts --ticker AAPL
```

This will call the following.

`https://data.sec.gov/api/xbrl/companyfacts/CIK##########.json`

This requires the CIK, not the ticker symbol. Currently, we do not have the cik attribute in the Stocks table, we'd have to
re-source the company data. Or we could call this endpoint first, get the CIK and then call the SEC endpoint.

`https://finnhub.io/api/v1/stock/filings?symbol=AAPL&token=<token>`

One of the fields in the response object (which returns an array of the latest filings:
```json
{
    "accessNumber": "0001193125-20-050884",
    "symbol": "AAPL",
    "cik": "320193",
    "form": "8-K",
    "filedDate": "2020-02-27 00:00:00",
    "acceptedDate": "2020-02-27 06:14:21",
    "reportUrl": "https://www.sec.gov/ix?doc=/Archives/edgar/data/320193/000119312520050884/d865740d8k.htm",
    "filingUrl": "https://www.sec.gov/Archives/edgar/data/320193/000119312520050884/0001193125-20-050884-index.html"
}
```
The CIK then needs to be 10 digits long, however many zeros needed in front, i.e. `320193` -> `000320193`. This could be
in a helper/utility class.

The company facts JSON format is an object with three attributes at the top level, `cik`, `entityName`, and `facts`.
The last one contains a `us-gaap` field, which contains the bulk of what we're interested in. Inside this are all the
Tags, or normalized metric names.

Now, we need to create a mapping between a standardized term around our business logic and the SEC tags. For example, 
for the metric of `Revenue`, a financial institution or bank would have `us-gaap:RevenuesNetOfInterestExpense`, 
whereas a technology company like AAPL, TSLA have `us-gaap:RevenueFromContractWithCustomerExcludingAssessedTax`. Actually,
TSLA actually has that and just `Revenues`, which is more extensive (the former only goes back until 2018?). So we may
need to manually change before running? Metrics like `NetIncome` seem to be a lot more consistent: `NetIncomeLoss`.

Inside of `Tradelens.Core/Constants/`, create a mapping to the tune of "<tradelens-tag>": "<sec-edgar-tag>", e.g.
```csharp
{
    "Revenue": "RevenueFromContractWithCustomerExcludingAssessedTax"
}
```

Next, note the nested structure of each Tag, i.e. `RevenuesNetOfInterestExpense.units.USD`, which contains an array with
a `fact` from each period. Each object contains key value pairs about the fact, including the value itself, and information
we need to "uniquely" identify it.

![sofi-secedgar-companyfacts](https://github.com/user-attachments/assets/8276cf99-b887-4a56-a31e-0bc8dec06a24)

Also take close note of the `start` and `end` dates, `start: "2020-01-01", end: "2020-06-30"` represents six months ended,
while we are only concerned with three months ended (quarterly) values such as `start: "2020-04-01", end: "2020-06-30"`,
and 12 months (annual). A few companies in other sectors such as consumer retail (WMT) have their fiscal years not as the 
standard calendar year, so need to make note of that as well.

Initial thoughts, have conditional clauses:

```
if `start` ends in '01-01' AND `end` ends in '12-31', then this is an annual metric
if `start` ends in '01-01' AND `end` ends in '03-31', then this is a quarterly metric
```

There are many instances where the same fact value from the same time period is repeated, while the `filed` dates are
different. These values correspond to the same fact we are concerned with, i.e. in the below picture, FY 2022 Revenue,
which was 1,573,535,000.

![sofi-secedgar-samefact](https://github.com/user-attachments/assets/71e6d508-97cb-4ec2-bb23-bdc2dece4b31)

This is due to these values being "picked up" in multiple filings, especially when being compared side-by-side with past periods.
The following is from SOFI's 2024 10-K, where the 2023 and 2022 full year values are also present. 

![sofi-2022-rev-repeat](https://github.com/user-attachments/assets/05bcb213-6126-4e34-9cb0-a8621618f0cf)

We can then presume that the 2023 and 2022 annual reports contain the 2022 FY revenue value as well, hence why we see
three repeated values in the company facts JSON.

The most important thing is the `start` and `end` date values, which are the same for all three (self-explanatory).
As stated before, Walmart's (retailer) Q1 FY2025 will have the start and end dates of '2024-02-01' to '2024-04-30' respectively.
Whereas in most other companies that use the calendar year will have '2025-01-01' to '2025-03-31'.

From the example above, the start and end date values are the exact same so we need an identifier to extract only one of
them. This could either be by the earliest filing, determined by the `filed` attribute, for example. Using the case
above, since "2023-03-01" is the earliest, we will extract that object.

Just for confirmation, here is Walmart's `Revenues` for the three-months-ended 2024-10-31.

![wmt-secedgar-revenue](https://github.com/user-attachments/assets/8a0fb9eb-599d-40ef-b8bf-e83911ff781e)

Since retailers like Walmart end their fiscal years on January 31 instead of December 31 to capture the tail end of the
holiday season, the above fact would be for FY 2025, not 2024. To be specific Walmart's Fiscal Year 2025 would run from
Feb 1, 2024 to Jan 31, 2025. In the first of the two objects, filed in "2024-12-06" (earlier), the `fy` and `fp` fields 
point to '2025' and 'Q3' respectively, so our extraction algorithm is correct.

The current decision is to have each of these objects seed into a single DB row in the Company Metric table.
```text
for each object to be extracted:
Ticker: ""
Stock, or new entity Company foreign key relationship
Cik: (get cik)
MetricName: is key from Core.Constants defined mapping
ParentMetric: will be defined as well, for example: EarningsPerShareBasic, EarningsPerShareDiluted => EPS
PeriodStartDate: `start`
PeriodEndDate: `end`
Value: `val`
Unit: defined in Mapping? i.e. Dollars, Units, Percent, etc.
FiscalYear: `fy`
FiscalPeriod: `fp`
Form: `form`
FiledDate: `filed`
Interval: infer from `fp` for `form`; (quarter/annual)
```

Note: PeriodStartDate should be nullable since for time-in-point metrics like `Assets` and `Liabilities`, there is no `start`.
