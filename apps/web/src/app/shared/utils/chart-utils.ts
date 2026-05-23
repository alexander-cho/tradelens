import { ChildMetricGroup, ValueDataAtEachPeriod } from '../models/fundamentals/company-fundamentals-response';
import { METRIC_DISPLAY_OVERRIDES } from '@tradelens/charting';
import { BARCHART_COLORS } from './barchart-colors';

/*
* Method to get slider timeline values. When only dealing with charting one metric at a time, there's no need to create
* the master timeline.
*/
export const createTimelineForSingleMetric = (valueDataList: ValueDataAtEachPeriod[]): {
  periodEndDates: string[],
  labels: string[]
} => {
  const periodEndDates: string[] = [];
  const labels: string[] = [];
  valueDataList.forEach(valueDataPoint => {
    periodEndDates.push(valueDataPoint.periodEndDate);
    let quarterFiscalYear = valueDataPoint.period + ' ' + valueDataPoint.fiscalYear;
    labels.push(quarterFiscalYear);
  });
  return {
    periodEndDates: periodEndDates,
    labels: labels
  }
}


// create a list/mapping of unique period combinations, this will go into a separate utils too
export const createMasterTimeline = (childMetricsList: ChildMetricGroup[]): {
  periodEndDates: string[],
  labels: string[]
} => {
  // create map: periodEndDate -> "Q1 2025" display label
  const periodMap = new Map<string, string>();
  childMetricsList.forEach(childMetric => {
    childMetric.data.forEach(x => {
      const dateKey = x.periodEndDate;
      const displayLabel = x.period + ' ' + x.fiscalYear;
      if (!periodMap.has(dateKey)) {
        periodMap.set(dateKey, displayLabel);
      }
    });
  });
  const sortedDates: string[] = Array.from(periodMap.keys()).sort((a, b) => {
    return new Date(a).getTime() - new Date(b).getTime();
  });
  return {
    periodEndDates: sortedDates,
    labels : sortedDates.map(date => periodMap.get(date)!)
  }
}


// now for each childMetricsGroup's data value ValueDataAtEachPeriod, check if there is a matching PeriodEndDate with
// each PeriodEndDate in the master timeline (separate utils)
export const alignDataToTimeline = (childMetricData: ChildMetricGroup, masterTimeline: string[]): (number | null)[] => {
  return masterTimeline.map(date =>{
    // for THIS date in the master timeline, find if this child metric has data
    const dataPoint: ValueDataAtEachPeriod | undefined = childMetricData.data.find(d => d.periodEndDate === date);
    return dataPoint ? dataPoint.value : null;
  });
}


/* Transform metric name, e.g. "CashAndDebt" -> "Cash And Debt", split between uppercase letters add a space except for first */
export const transformMetricName = (originalMetric: string): string => {
  if (METRIC_DISPLAY_OVERRIDES[originalMetric]) {
    return METRIC_DISPLAY_OVERRIDES[originalMetric];
  }
  const splitMetricName = originalMetric.split('');
  let newMetricName = '';
  if (splitMetricName != null) {
    for (let i = 0; i < splitMetricName.length; i++) {
      // check for uppercase letters, except for the first one
      if (i !== 0 && splitMetricName[i] === splitMetricName[i].toUpperCase() && splitMetricName[i] !== splitMetricName[i].toLowerCase()) {
        newMetricName = newMetricName + ' ' + splitMetricName[i];
      } else {
        newMetricName = newMetricName + '' + splitMetricName[i];
      }
    }
  }
  return newMetricName;
}


// generate the necessary colors for each individual child metric
export const generateUniqueColors = (numChildMetrics: number): string[] => {
  let colorsList: string[] = [];
  for (let i = 0; i < numChildMetrics; i++) {
    const barColor = BARCHART_COLORS[Math.floor(Math.random() * BARCHART_COLORS.length)];
    colorsList.push(barColor);
  }
  return colorsList;
}


// create gradient for bars
export const createGradient = (ctx: any, chartArea: any, barColor: string) => {
  const gradient = ctx.createLinearGradient(0, chartArea.bottom, 0, chartArea.top);

  const insertAlphaValueToRgba = (barColor: string, alphaValue: number) => {
    // rgba string from utils has no alpha value, insert it right before closing ')' and put a ',' in front as well
    const parts = barColor.split(')');
    return parts[0] + ' ,' + alphaValue + parts[1] + ')';
  }

  // color fades out towards the x-axis
  const bottomColor = insertAlphaValueToRgba(barColor, 0.3);
  const topColor = insertAlphaValueToRgba(barColor, 0);

  gradient.addColorStop(1, bottomColor);
  gradient.addColorStop(0, topColor);

  return gradient;
};
