export type CompanyProfile = {
  symbol: string;
  price: number;
  marketCap: number;
  lastDividend: number;
  range: string;
  change: number;
  changePercentage: number;
  volume: number;
  exchange: string;
  website: string;
  image: string;
};


export type KeyMetrics = {
  symbol: string;
  enterpriseValueTtm: number;
  returnOnInvestedCapitalTtm: number;
  currentRatio: number;
  netDebtToEbitdaTtm: number;
};


export type FinancialRatios = {
  symbol: string;
  debtToEquityRatioTtm: number;
  grossProfitMarginTtm: number;
  priceToEarningsRatioTtm: number;
  forwardPriceToEarningsGrowthRatioTtm: number;
  priceToSalesRatioTtm: number;
  priceToBookRatioTtm: number;
  priceToFreeCashFlowRatioTtm: number;
};

