# Handling values when a company reports metrics by a different name/category or not at all in one or more periods

Something odd caught my eye earlier today.

<img alt="Duolingo and logo" height="30" src="https://github.com/user-attachments/assets/3f9b7d6d-6598-4e78-a860-342bd9b4c8aa" width="200"/>

<img alt="Duolingo Revenue Minus Subscriptions Quarterly" height="250" src="https://github.com/user-attachments/assets/5fdf9e5e-b8a0-4fd0-8009-bf440578800d" width="250"/>

Duolingo has only reported an "In App Purchases" segment in their revenue breakdown for the past 8 quarters. In the
above
photo, there are 8 bars for it alright, but they're in the wrong positions. I checked the data to make sure there wasn't
a mistake, and it was fine as seen below-here is the latest quarter:

```json
{
  "Ticker": "DUOL",
  "Period": "Q3",
  "Year": 2025,
  "Interval": "quarterly",
  "Metric": "InAppPurchases",
  "ParentMetric": "RevenueBreakdownMinusSubscriptions",
  "Value": 11096000,
  "Section": "KPI",
  "SourcedFrom": "10-Q",
  "PeriodEndDate": "2025-09-30",
  "Unit": "Dollars"
}
```

So the 8 quarters represent those of Q4 2023 through Q3 2025 but are just pushed to the beginning of the time period
axis.

I figured my backend was querying these metrics just fine based on the user-requested parameters, so I had to dig into
my
charting logic client-side. I'll be referring to the following file:

`client-ng/src/app/shared/components/company-metric-chart/company-metric-chart.component.ts`

In the else if part of my chart generating logic where I read to see if the Parent (main) metric has child metrics, i.e.
it
is a breakdown of some sort, I am mapping the x-axis labels (Q1 2023, Q2 2023, ...) to the very first child metric
whatever
it is.

```
labels: childMetricsRead[0].data.map(x => x.period + ' ' + x.fiscalYear),
```

So `childMetricsRead[0]` may be `Advertising` here, which has recorded metrics for each quarter as opposed to
`In App Purchases`.
Chart.js creates 19 x-axis labels based on that, but when it renders the `In App Purchases` with 8 data points, it just
places them in `[0,1,2,3,4,5,6,7]` because that is the array that it receives. It does not know that it should rather go
into positions `[11,12,13,14,15,16,17,18]`.

I need to create some sort of "master timeline" with all the unique period combinations accounted for. I will create a
method
inside the class to handle this:

```ts
export class CompanyMetricChartComponent implements AfterViewInit {
  // ...
  // ...
  // create a list of unique period combinations
  private createMasterTimeline = (childMetricsList: ChildMetricGroup[]) => {
    let uniquePeriods: string[] = [];
    childMetricsList.forEach(childMetric => {
      childMetric.data.forEach(x => {
        let xValue = x.period + ' ' + x.fiscalYear;
        if (!uniquePeriods.includes(xValue)) {
          uniquePeriods.push(xValue);
        }
      })
    });
    return uniquePeriods.sort;
  }
  // ...
  // ...
}
```

But wait. How can I sort by these Quarter + FiscalYear strings? I'd have to create more custom logic for it. I figured I
will just update my `ValueDataAtEachPeriod` type to include the PeriodEndDate, which is a Date object that I can much
more easily sort. I will create a map that puts the PeriodEndDate to its matching Quarter + FiscalYear string.

`client-ng/src/app/shared/models/fundamentals/company-fundamentals-response.ts`

```ts
export type ValueDataAtEachPeriod = {
  // period: string;
  // fiscalYear: string;
  // value: number;
  periodEndDate: string;  // <---- Added this
}
```

Add it to our backend DTO as well, as seen at `backend/src/API/DTOs/CompanyMetricDto.cs`

```csharp
public class MetricDataPoint
{
    //public string? Period { get; set; }
    //public int FiscalYear { get; set; }
    //public decimal Value { get; set; }
    public DateOnly PeriodEndDate { get; set; }  // <---- Added this
}
```

```ts
export class CompanyMetricChartComponent implements AfterViewInit {
  // ...
  // ...
  private createMasterTimeline = (childMetricsList: ChildMetricGroup[]): {
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
      })
    });
    const sortedDates: string[] = Array.from(periodMap.keys()).sort((a, b) => {
      return new Date(a).getTime() - new Date(b).getTime();
    });
    return {
      // periodEndDates: sortedDates,
      // labels : sortedDates.map(date => periodMap.get(date)!)
    }
  }
// ...
// ...
}
```

Now using the master timeline which has the number of x-axis points (periods) needed in the case of 
Duolingo Revenue Breakdown Minus Subscriptions (Quarterly), for each childMetric list, we need to see if a PeriodEndDate for
each point in the master timeline exists in the childMetric list.

![Duolingo Revenue Breakdown Quarters Mappings](https://github.com/user-attachments/assets/7fd59c62-e871-4bb8-826c-01d155e79a27)

As seen in this graphic above, where there is no mapping between the periodEndDate of the masterTimeline and the
ChildMetricGroup (In App Purchases in this case), we need to insert a *null* value at that time slot.

```ts
export class CompanyMetricChartComponent implements AfterViewInit {
  // ...
  // ...
  private alignDataToTimeline = (childMetricData: ChildMetricGroup, masterTimeline: string[]): (number | null)[] => {
    return masterTimeline.map(date => {
      // for THIS date in the master timeline, find if this child metric has data
      const dataPoint: ValueDataAtEachPeriod | undefined = childMetricData.data.find(d => d.periodEndDate === date);
      return dataPoint ? dataPoint.value : null;
    });
  }
  // ...
  // ...
}
```

Here, I create a function `alignDataToTimeline()` which takes in a `ChildMetricGroup` and the masterTimeline. The return
type is a list of values that can be a number or null as discussed before. For each periodEndedDate, we will check if
there exists a data point `ValueDataAtEachPeriod` that has the matching periodEndedDate. Within the chart config, this
function will run 'x' amount of times where 'x' is the number of `ChildMetricGroup`'s. Let's deploy these newly created
mechanisms to the chart config options.

Initialize the master timeline object first right before creating the child metrics chart (else if block):

```ts
const masterTimeline = this.createMasterTimeline(childMetricsRead);
```

Then for the `labels` and `data` of the chart config I can use:

```ts
labels: masterTimeline.labels
```

```ts
data: this.alignDataToTimeline(childMetric, masterTimeline.periodEndDates)
```

<img alt="Corrected Duolingo Revenue Breakdown Quarters Mappings" height="250" src="https://github.com/user-attachments/assets/d4e98dcb-acac-46dc-8d4a-42144aa99763" width="250"/>

Fixed.