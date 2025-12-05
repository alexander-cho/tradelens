Hi, here is a 10-K attached.

Here are the metrics I am looking for. Since this is yearly, you will get the year ended values (12-31)!

**Financials**
(Can find in section like Condensed Consolidated Statements of Operations and Comprehensive Income, or similar)
Revenue,
CostOfRevenue
NetIncome,
InterestIncome,
NoninterestIncome (maybe not there)
OperatingExpenses: [ResearchAndDevelopment, SalesAndMarketing, GeneralAndAdministrative],
AdjustedEbitda,
EPS: [Basic, Diluted]
SharesOutstanding


(Can find in section like Condensed Consolidated Balance Sheets, or similar)
TotalAssets,
TotalLiabilities,
TotalStockholdersEquity
CashAndDebt: [CashAndCashEquivalents, Debt], (could be marked as "Long term debt" or similar)


(Can find in section like Condensed Consolidated Statements of Stockholders’ Equity, or similar)
StockBasedCompensation,


**KPI**
(Can find in section like Management’s Discussion and Analysis of Financial Condition and Results of Operations, Key Business Metrics, or similar)
RevenueBreakdown: [Transaction, SubscriptionAndServices, Other]
VerifiedUsers
MonthlyTransactingUsers
AssetsOnPlatform
TradingVolume
AssetsOnPlatformBreakdown: [Bitcoin, Ethereum, XRP, Solana, USDC, Other]
TradingVolumeBreakdown: [Consumer, Institutional]
TradingVolumeByCryptoAsset: [Bitcoin, Ethereum, XRP, USDT, Other] (Percent)
TransactionRevenueBreakdown: [Consumer, Institutional, Other]
TransactionRevenueByCryptoAsset: [Bitcoin, XRP, Ethereum, Solana, Other] (Percent)
SubscriptionAndServicesRevenueBreakdown: [Stablecoin, BlockchainRewards, InterestAndFinanceFeeIncome, Other]


So each CompanyMetric will become a JSON object with the attributes

Ticker,Period,Year,Interval,Metric,ParentMetric,Value,Section,SourcedFrom,PeriodEndDate,Unit

Ticker,Period,Year,Interval,SourcedFrom,PeriodEndDate will all be common since they are coming from this same document
e.g. SOFI, FY, 2024, annual, 10-K, 2024-12-31

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