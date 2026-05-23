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
import {
  alignDataToTimeline,
  createGradient,
  createMasterTimeline,
  createTimelineForSingleMetric,
  generateUniqueColors,
  transformMetricName
} from '../../../utils/chart-utils';
import { NONSTACKED_METRICS } from '@tradelens/charting';

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
    // add the last period end date label in case total is not divisible by whichever number defined in the if-statement
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
      this.fullTimeline.set(createTimelineForSingleMetric(this.valueData()!));
    } else {
      this.fullTimeline.set(createMasterTimeline(this.childMetrics()!));
    }
  }

  // modify later and use transformMetricName() for the inner logic here (DONE)
  protected spacedMetricName: Signal<string | undefined> = computed(() => {
    return transformMetricName(this.metricName()!);
  });

  // use this to give the expanded chart a different chart canvas id. or else it will try to use the same id from the
  // chart that the user clicked the expand arrow on from the dashboard view, which is already in use.
  protected metricNameForExpandedChart: Signal<string | undefined> = computed(() => {
    return this.spacedMetricName() + 'expanded';
  })

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
      const uniqueBarColors: string[] = generateUniqueColors(childMetricsRead.length);

      const masterTimeline = createMasterTimeline(childMetricsRead);

      if (!NONSTACKED_METRICS.has(metricNameRead!)) {
        this.chart = new Chart(chartCanvasId, {
          type: 'bar',
          data: {
            labels: masterTimeline.labels,
            datasets: childMetricsRead.map((childMetric, index) => ({
              label: transformMetricName(childMetric.metricName),
              data: alignDataToTimeline(childMetric, masterTimeline.periodEndDates),
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
            labels: masterTimeline.labels,
            datasets: childMetricsRead.map((childMetric, index) => ({
              label: transformMetricName(childMetric.metricName),
              data: alignDataToTimeline(childMetric, masterTimeline.periodEndDates),
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
