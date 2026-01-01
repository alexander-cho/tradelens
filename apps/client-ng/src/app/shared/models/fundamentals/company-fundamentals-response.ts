export type CompanyFundamentalsResponse = {
  metricData: Metric[];
}

export type Metric = {
  metricName: string;
  data?: ValueDataAtEachPeriod[];
  childMetrics?: ChildMetricGroup[];
}

export type ChildMetricGroup = {
  metricName: string;
  data: ValueDataAtEachPeriod[];
}

export type ValueDataAtEachPeriod = {
  period: string;
  fiscalYear: string;
  value: number;
  periodEndDate: string;
}
