export type CompanyProfile = {
  symbol: string;
  price: number;
  marketCap: number;
  beta: number;
  lastDividend: number;
  range: string;
  change: number;
  changePercentage: number;
  volume: number;
  averageVolume: number;
  companyName: string;
  currency: string;
  cik: string;
  isin: string;
  cusip: string;
  exchangeFullName: string;
  exchange: string;
  industry: string;
  website: string;
  description: string;
  ceo: string;
  sector: string;
  country: string;
  fullTimeEmployees: string; // FMP returns this as a string
  phone: string;
  address: string;
  city: string;
  state: string;
  zip: string;
  image: string;
  ipoDate: string;
  defaultImage: boolean;
  isEtf: boolean;
  isActivelyTrading: boolean;
  isAdr: boolean;
  isFund: boolean;
};



export type KeyMetrics = {
  symbol: string;
  enterpriseValueTtm: number;
  returnOnInvestedCapitalTtm: number;
  evToEbitdaTtm: number;
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

