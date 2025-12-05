import { AfterViewInit, Component, computed, inject, Signal, signal, WritableSignal } from '@angular/core';
import { NZ_MODAL_DATA } from 'ng-zorro-antd/modal';
import { ChildMetricGroup, ValueDataAtEachPeriod } from '../../../models/fundamentals/company-fundamentals-response';
import { Chart, ScriptableContext } from 'chart.js/auto';
import { BARCHART_COLORS } from '../../../utils/barchart-colors';
import { NONSTACKED_METRICS } from '../../../utils/nonstacked-metrics';
import { METRIC_DISPLAY_OVERRIDES } from '../../../utils/metric-display-names';

@Component({
  selector: 'app-expand-company-metric-chart-modal',
  imports: [],
  templateUrl: './expand-company-metric-chart-modal.component.html',
  styleUrl: './expand-company-metric-chart-modal.component.scss'
})
export class ExpandCompanyMetricChartModalComponent implements AfterViewInit {
  data = inject(NZ_MODAL_DATA);

  ticker: WritableSignal<string> = signal<string>(this.data.tickerSymbol);
  metricName: WritableSignal<string> = signal<string>(this.data.metricNameForModal);
  valueData: WritableSignal<ValueDataAtEachPeriod[] | undefined> = signal<ValueDataAtEachPeriod[] | undefined>(this.data.dataForModal);
  childMetrics: WritableSignal<ChildMetricGroup[] | undefined> = signal<ChildMetricGroup[] | undefined>(this.data.childMetricsForModal);

  protected chart?: Chart;

  ngAfterViewInit() {
    this.createChart();
  }

  // modify later and use transformMetricName() for the inner logic here
  protected spacedMetricName: Signal<string | undefined> = computed(() => {
    if (METRIC_DISPLAY_OVERRIDES[this.metricName()!]) {
      return METRIC_DISPLAY_OVERRIDES[this.metricName()!];
    }
    // split metric names between uppercase letters
    const splitMetricName = this.metricName()?.split('');
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
  });

  // use this to give the expanded chart a different chart canvas id. or else it will try to use the same id from the
  // chart that you clicked the expand arrow on from the dashboard view, which is already in use.
  protected metricNameForExpandedChart: Signal<string | undefined> = computed(() => {
    return this.spacedMetricName() + 'expanded';
  })

  // separate utils!
  private transformMetricName = (originalMetric: string) => {
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

  // create a list/mapping of unique period combinations, this will go into a separate utils too
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
  private alignDataToTimeline = (childMetricData: ChildMetricGroup, masterTimeline: string[]): (number | null)[] => {
    return masterTimeline.map(date =>{
      // for THIS date in the master timeline, find if this child metric has data
      const dataPoint: ValueDataAtEachPeriod | undefined = childMetricData.data.find(d => d.periodEndDate === date);
      return dataPoint ? dataPoint.value : null;
    });
  }

  private createChart() {
    const barColor = BARCHART_COLORS[Math.floor(Math.random() * BARCHART_COLORS.length)];
    const metricNameRead = this.metricName();

    const metricDisplayName = this.spacedMetricName();
    const chartCanvasId = this.metricNameForExpandedChart();

    const dataRead = this.valueData();
    const childMetricsRead = this.childMetrics();

    if (!chartCanvasId || !metricDisplayName || (!dataRead && !childMetricsRead)) {
      return;
    }

    // create gradient for bar
    const createGradient = (ctx: any, chartArea: any, barColor: string) => {
      const gradient = ctx.createLinearGradient(0, chartArea.bottom, 0, chartArea.top);

      const bottomColor = insertAlphaValueToRgba(barColor, 0.3);
      const topColor = insertAlphaValueToRgba(barColor, 0);

      gradient.addColorStop(1, bottomColor);
      gradient.addColorStop(0, topColor);

      return gradient;
    };

    const insertAlphaValueToRgba = (barColor: string, alphaValue: number) => {
      // rgba string from utils has no alpha value, insert it right before closing ')' and put a ',' in front as well
      const parts = barColor.split(')');
      return parts[0] + ' ,' + alphaValue + parts[1] + ')';
    }

    this.chart?.destroy();

    // for chart creation with child metrics, consider using a signal for { stacked: true/false } depending on which metric
    // represents a collective versus separate: for example, Cash vs Debt should not be stacked since they are separate
    // rather than additive like Operating Expenses = S&M + R&D + G&A. This list of metrics will be in /shared/utils/

    if (dataRead != null) {
      this.chart = new Chart(chartCanvasId, {
        type: 'bar',
        data: {
          labels: dataRead.map(x => x.period + ' ' + x.fiscalYear),
          datasets: [
            {
              label: metricDisplayName,
              data: dataRead.map(x => x.value),
              backgroundColor: function (context: ScriptableContext<'bar'>) {
                const chart = context.chart;
                const { ctx, chartArea } = chart;
                if (!chartArea) {
                  return;
                }
                return createGradient(ctx, chartArea, barColor);
              },
              borderColor: barColor,
              borderWidth: 2,
              borderRadius: 0.5
            }
          ],
        },
        options: {
          scales: {
            x: {},
            y: {
              beginAtZero: true
            },
          },
          plugins: {
            title: {
              text: `${ this.ticker() } ${ metricNameRead }`,
              display: false
            }
          },
          // width / height
          aspectRatio: 1.7
        },
      });
    } else if (childMetricsRead != null) {
      // generate the necessary colors for each individual child metric
      const generateUniqueColors = (numChildMetrics: number): string[] => {
        let colorsList: string[] = [];
        for (let i = 0; i < numChildMetrics; i++) {
          const barColor = BARCHART_COLORS[Math.floor(Math.random() * BARCHART_COLORS.length)];
          colorsList.push(barColor);
        }
        return colorsList;
      }

      const uniqueBarColors: string[] = generateUniqueColors(childMetricsRead.length);

      const masterTimeline = this.createMasterTimeline(childMetricsRead);

      if (!NONSTACKED_METRICS.has(metricNameRead!)) {
        this.chart = new Chart(chartCanvasId, {
          type: 'bar',
          data: {
            // labels: childMetricsRead[0].data.map(x => x.period + ' ' + x.fiscalYear),
            labels: masterTimeline.labels,
            datasets: childMetricsRead.map((childMetric, index) => ({
              label: this.transformMetricName(childMetric.metricName),
              // data: childMetric.data.map(x => x.value),
              data: this.alignDataToTimeline(childMetric, masterTimeline.periodEndDates),
              backgroundColor: function (context: ScriptableContext<'bar'>) {
                const chart = context.chart;
                const { ctx, chartArea } = chart;
                if (!chartArea) {
                  return;
                }
                return createGradient(ctx, chartArea, uniqueBarColors[index]);
              },
              borderColor: uniqueBarColors[index],
              borderWidth: 2,
              borderRadius: 0.5,
            }))
          },
          options: {
            scales: {
              x: {
                stacked: true
              },
              y: {
                beginAtZero: true,
                stacked: true
              },
            },
            plugins: {
              title: {
                text: `${ this.ticker() } ${ metricNameRead }`,
                display: false
              }
            },
            // width / height
            aspectRatio: 1.7
          },
        });
      } else {
        this.chart = new Chart(chartCanvasId, {
          type: 'bar',
          data: {
            // labels: childMetricsRead[0].data.map(x => x.period + ' ' + x.fiscalYear),
            labels: masterTimeline.labels,
            datasets: childMetricsRead.map((childMetric, index) => ({
              label: this.transformMetricName(childMetric.metricName),
              // data: childMetric.data.map(x => x.value),
              data: this.alignDataToTimeline(childMetric, masterTimeline.periodEndDates),
              backgroundColor: function (context: ScriptableContext<'bar'>) {
                const chart = context.chart;
                const { ctx, chartArea } = chart;
                if (!chartArea) {
                  return;
                }
                return createGradient(ctx, chartArea, uniqueBarColors[index]);
              },
              borderColor: uniqueBarColors[index],
              borderWidth: 2,
              borderRadius: 0.5,
            }))
          },
          options: {
            scales: {
              x: {
                stacked: false
              },
              y: {
                beginAtZero: true,
                stacked: false
              },
            },
            plugins: {
              title: {
                text: `${ this.ticker() } ${ metricNameRead }`,
                display: false
              }
            },
            // width / height
            aspectRatio: 1.7
          },
        });
      }
    }
  }
}
