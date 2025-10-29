export type BarAggregates = {
  ticker: string;
  queryCount: number;
  resultsCount: number;
  adjusted: boolean;
  results: Bar[];
}

export type Bar = {
  v: number;
  vw: number;
  o: number;
  c: number;
  h: number;
  l: number;
  t: number;
  n: number;
}

export type RelatedCompanies = {
  ticker: string;
  status: string;
  results: RelatedCompany[];
}

export type RelatedCompany = {
  ticker: string;
}
