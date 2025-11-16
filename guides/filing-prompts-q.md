Hi, here is a 10-Q attached.

Here are the metrics I am looking for. Since this is quarterly, you will get the last three months ended values!

**Financials**

(Can find in section like Condensed Consolidated Statements of Operations and Comprehensive Income, or similar)
Revenue, 
GrossProfit, 
NetIncome,
InterestIncome,
NoninterestIncome
OperatingExpenses: [ResearchAndDevelopment, SalesAndMarketing, GeneralAndAdministrative],
DepreciationAndAmortization
AdjustedEbitda,
EPS: [Basic, Diluted]
SharesOutstanding

(Can find in section like Condensed Consolidated Balance Sheets, or similar)
TotalAssets,
TotalLiabilities,
CashAndDebt: [CashAndCashEquivalents, Debt],

(Can find in section like Condensed Consolidated Statements of Cash Flows, or similar)
FreeCashFlow,
DepreciationAndAmortization

(Can find in section like Condensed Consolidated Statements of Stockholders’ Equity, or similar)
TotalStockholdersEquity,
StockBasedCompensation,

**KPI**
(Can find in section like Management’s Discussion and Analysis of Financial Condition and Results of Operations, Key Business Metrics, or similar)

TotalDeposits
NetInterestMargin (as a Percent),
Members,
GalileoAccounts (Could be listed as Technology Platform accounts),
Products: [Lending, FinancialServices],
RevenueBySegment: [Lending, TechnologyPlatform, FinancialServices],
ContributionProfit: [Lending, TechnologyPlatform, FinancialServices],
LendingProducts: [PersonalLoans, StudentLoans, HomeLoans],
FinancialServicesProducts: [Money, Invest, CreditCard, ReferredLoans, Relay, AtWork],
NetChargeOffRates: [PersonalLoans, StudentLoans] (as a Percent),
OriginationVolume: [PersonalLoans, StudentLoans, HomeLoans]


So each CompanyMetric will become a json object with the attributes

Ticker,Period,Year,Interval,Metric,ParentMetric,Value,Section,SourcedFrom,PeriodEndDate,Unit

Ticker,Period,Year,Interval,SourcedFrom,PeriodEndDate will all be common since they are coming from this same document
e.g. SOFI, Q3, 2025, quarterly, 10-Q, 2025-09-30

For the Metric, ParentMetric keep as PascalCase as defined above

For the Values could you get the raw number, without rounding (i.e. thousands, millions)
Unit can be (Dollars, Percent, etc.) where applicable

Some of the names may show up a little bit differently e.g. "Research And Development" vs "Technology And Product Development"

For the Parent Metric, for example
Operating Expenses: [ResearchAndDevelopment, SalesAndMarketing, GeneralAndAdministrative]
each one in the list will be its own CompanyMetric object, with ParentMetric Pointing to OperatingExpenses
If it's just a single one like Revenue, or SharesOutstanding, parent metric is the same.

The Section attribute will point to whether it came from **Financials** or **KPI**

If you can't find the matching metric, you do not have to create an object for it, don't force it.