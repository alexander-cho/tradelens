import { ValueDataAtEachPeriod } from './company-fundamentals-response';

export type CompanyMetricDto = {
  metricName: string;
  data: ValueDataAtEachPeriod[];
}
