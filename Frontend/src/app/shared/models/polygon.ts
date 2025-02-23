export type BarAggregates = {
  ticker: string;
  queryCount: number;
  resultsCount: number;
  adjusted: boolean;
  results: Bar[];
}

type Bar = {
  v: number;
  vw: number;
  o: number;
  c: number;
  h: number;
  l: number;
  t: bigint;
  n: number;
}

export type RelatedCompanies = {
  ticker: string;
  status: string;
  results: RelatedCompany[];
}

type RelatedCompany = {
  ticker: string;
}
