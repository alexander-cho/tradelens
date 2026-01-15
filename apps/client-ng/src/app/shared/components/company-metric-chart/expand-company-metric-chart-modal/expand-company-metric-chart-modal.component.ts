import {
  AfterViewInit,
  Component,
  computed,
  effect,
  inject,
  OnInit,
  Signal,
  signal,
  WritableSignal
} from '@angular/core';
import { NZ_MODAL_DATA } from 'ng-zorro-antd/modal';
import { ChildMetricGroup, ValueDataAtEachPeriod } from '../../../models/fundamentals/company-fundamentals-response';
import { Chart, ScriptableContext } from 'chart.js/auto';
import { CompanyDashboardService } from '../../../../core/services/company-dashboard.service';
import { NzRadioComponent, NzRadioGroupComponent } from 'ng-zorro-antd/radio';
import { FormsModule } from '@angular/forms';
import { NzIconDirective } from 'ng-zorro-antd/icon';
import { RouterLink } from '@angular/router';
import { NzMarks, NzSliderComponent } from 'ng-zorro-antd/slider';

import { BARCHART_COLORS } from '../../../utils/barchart-colors';
import { NONSTACKED_METRICS } from '@tradelens/charting';
import { METRIC_DISPLAY_OVERRIDES } from '@tradelens/charting';

@Component({
  selector: 'app-expand-company-metric-chart-modal',
  imports: [
    NzRadioComponent,
    NzRadioGroupComponent,
    FormsModule,
    NzIconDirective,
    RouterLink,
    NzSliderComponent
  ],
  templateUrl: './expand-company-metric-chart-modal.component.html',
  styleUrl: './expand-company-metric-chart-modal.component.scss'
})
export class ExpandCompanyMetricChartModalComponent implements OnInit, AfterViewInit {
  data = inject(NZ_MODAL_DATA);

  private companyDashboardService = inject(CompanyDashboardService);

  ticker: WritableSignal<string | undefined> = signal<string | undefined>(this.data.tickerForModal);
  metricName: WritableSignal<string> = signal<string>(this.data.metricNameForModal);
  valueData: WritableSignal<ValueDataAtEachPeriod[] | undefined> = signal<ValueDataAtEachPeriod[] | undefined>(undefined);
  childMetrics: WritableSignal<ChildMetricGroup[] | undefined> = signal<ChildMetricGroup[] | undefined>(undefined);
  interval: WritableSignal<string> = signal<string>('quarterly');
  from: WritableSignal<string | undefined> = signal<string | undefined>(undefined);
  to: WritableSignal<string | undefined> = signal<string | undefined>(undefined);
  fullTimeline: WritableSignal<{ periodEndDates: string[], labels: string[] } | undefined> = signal<{
    periodEndDates: string[],
    labels: string[]
  } | undefined>(undefined);
  sliderRange: WritableSignal<number[] | undefined> = signal<number[] | undefined>(undefined);

  sliderTimelineMarks: Signal<NzMarks | null> = computed(() => {
    if (!this.fullTimeline()) {
      return null;
    }

    const marks: NzMarks = {};
    this.fullTimeline()?.labels.forEach((label, index) => {
      if (this.interval() == 'quarterly') {
        if (index % 4 == 0) {
          marks[index] = label;
        }
      } else if (this.interval() == 'annual') {
        marks[index] = label;
      }
    });
    // add the last period end date label if not divisible by 5
    const lastIndex = this.fullTimeline()?.labels.length! - 1;
    marks[lastIndex] = this.fullTimeline()!.labels[lastIndex];
    return marks;
  });

  protected chart?: Chart;

  ngOnInit() {
    this.sliderRange.set([0, this.fullTimeline()!.periodEndDates.length - 1]);
  }

  ngAfterViewInit() {
    this.getMetricsAndCreateChart();
  }

  // Upon component open, we need to get the full period timeline to create the slider with min/max.
  // If interval changes, need to re-create full period timeline.
  // To do this, call getCompanyMetrics() service method without specifying the from and to query params.

  chartChangeEffect = effect(() => {
    this.from();
    this.to();
    this.getMetricsAndCreateChart();
  });

  intervalChangeEffect = effect(() => {
    this.interval();
    this.getFullTimelineAndCreateChart();
  });

  onSliderChange(indices: number[]) {
    this.from.set(this.fullTimeline()?.periodEndDates[indices[0]]);
    this.to.set(this.fullTimeline()?.periodEndDates[indices[1]]);
  }

  getMetricsAndCreateChart() {
    this.companyDashboardService.getCompanyMetricsGroupedByParent(this.ticker(), this.interval(), [this.metricName()], this.from(), this.to()).subscribe({
      next: response => {
        this.valueData.set(response.metricData[0].data);
        this.childMetrics.set(response.metricData[0].childMetrics);
        this.createChart();
      },
      error: err => console.log(err)
    });
  }

  getFullTimelineAndCreateChart() {
    this.companyDashboardService.getCompanyMetricsGroupedByParent(this.ticker(), this.interval(), [this.metricName()]).subscribe({
      next: response => {
        this.valueData.set(response.metricData[0].data);
        this.childMetrics.set(response.metricData[0].childMetrics);
        this.getFullTimeline();
        this.resetSliderRangeForNewTimeline();
        this.createChart();
      },
      error: err => console.log(err)
    });
  }

  resetSliderRangeForNewTimeline() {
    const timelineLength = this.fullTimeline()?.periodEndDates.length;
    if (timelineLength) {
      this.sliderRange.set([0, timelineLength - 1]);
      // reset from/to values as well to match
      this.from.set(this.fullTimeline()?.periodEndDates[0]);
      this.to.set(this.fullTimeline()?.periodEndDates[timelineLength - 1]);
    }
  }

  // get indexable timeline with period end dates and labels to create slider
  getFullTimeline() {
    if (this.valueData() != null) {
      this.fullTimeline.set(this.createTimelineForSingleMetric(this.valueData()!));
    } else {
      this.fullTimeline.set(this.createMasterTimeline(this.childMetrics()!));
    }
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
  // chart that the user clicked the expand arrow on from the dashboard view, which is already in use.
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

  private createTimelineForSingleMetric = (valueDataList: ValueDataAtEachPeriod[]): {
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
      labels: sortedDates.map(date => periodMap.get(date)!)
    }
  }

  // now for each childMetricsGroup's data value ValueDataAtEachPeriod, check if there is a matching PeriodEndDate with
  // each PeriodEndDate in the master timeline (separate utils)
  private alignDataToTimeline = (childMetricData: ChildMetricGroup, masterTimeline: string[]): (number | null)[] => {
    return masterTimeline.map(date => {
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
          responsive: true,
          maintainAspectRatio: false,
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
          }
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
            responsive: true,
            maintainAspectRatio: false,
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
            }
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
            responsive: true,
            maintainAspectRatio: false,
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
            }
          },
        });
      }
    }
  }
}
