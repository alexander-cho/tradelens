export type CompanyFundamentalsResponse = {
  metricData: Metric[];
}

export type Metric = {
  metricName: string;
  data: ValueDataAtEachPeriod[];
}

export type ValueDataAtEachPeriod = {
  period: string;
  fiscalYear: string;
  value: number;
}
