Hi, here is a 10-Q attached.

Here are the metrics I am looking for. Since this is quarterly, you will get the last three months ended values!

**Financials**
(Can find in section like Condensed Consolidated Statements of Operations and Comprehensive Income, or similar)
Revenue,
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
TotalStockholdersEquity
CashAndDebt: [CashAndCashEquivalents, Debt],


(Can find in section like Condensed Consolidated Statements of Cash Flows, or similar)
FreeCashFlow (exclude if not visible, especially for financial companies),
DepreciationAndAmortization

(Can find in section like Condensed Consolidated Statements of Stockholders’ Equity, or similar)
StockBasedCompensation,


**KPI**
(Can find in section like Management’s Discussion and Analysis of Financial Condition and Results of Operations, Key Business Metrics, or similar)

FundedCustomers
TotalPlatformAssets (could be AUC but keep TotalPlatformAssets)
NetDeposits
AverageRevenuePerUser
GoldSubscribers
AssetsBreakdown: [Equites, Cryptocurrencies, OptionsAndFutures, RegisteredInvestmentAdvisorAssets, CashHeld, Receivables]
RevenueBreakdown: [TransactionBased, NetInterest, Other]
TransactionBasedRevenueBreakdown: [Options, Cryptocurrencies, Equities, Other]


So each CompanyMetric will become a JSON object with the attributes

Ticker,Period,Year,Interval,Metric,ParentMetric,Value,Section,SourcedFrom,PeriodEndDate,Unit

Ticker,Period,Year,Interval,SourcedFrom,PeriodEndDate will all be common since they are coming from this same document
e.g. SOFI, Q3, 2025, quarterly, 10-Q, 2025-09-30

For 'Metric', 'ParentMetric' keep as PascalCase as defined above

For 'Value' attribute, get the FULL number, instead of rounded one

'Unit' can be (Dollars, Percent, Units, etc.) where applicable

Some of the names may show up a little bit differently e.g. "Research And Development" vs "Technology And Product Development"

For the ParentMetric, for example
Operating Expenses: [ResearchAndDevelopment, SalesAndMarketing, GeneralAndAdministrative]
each one in the list will be its own 'Metric' object, with 'ParentMetric' pointing to OperatingExpenses
If it's just a single one like Revenue, or SharesOutstanding, ParentMetric is the same.

The 'Section' attribute will point to whether it came from **Financials** or **KPI**

If you can't find the matching metric, you do not have to create an object for it, don't force it.