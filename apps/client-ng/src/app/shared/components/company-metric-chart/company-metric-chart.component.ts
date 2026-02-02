import { Component, effect, input, InputSignal, AfterViewInit, computed, Signal, inject } from '@angular/core';
import { ChildMetricGroup, ValueDataAtEachPeriod } from '../../models/fundamentals/company-fundamentals-response';
import { Chart, ScriptableContext } from 'chart.js/auto';
import { NzCardComponent } from 'ng-zorro-antd/card';
import { NzIconDirective } from 'ng-zorro-antd/icon';
import { NzModalService } from 'ng-zorro-antd/modal';
import {
  ExpandCompanyMetricChartModalComponent
} from './expand-company-metric-chart-modal/expand-company-metric-chart-modal.component';
import { BARCHART_COLORS } from '../../utils/barchart-colors';
import {
  alignDataToTimeline,
  createGradient,
  createMasterTimeline, generateUniqueColors,
  transformMetricName
} from '../../utils/chart-utils';
import { NONSTACKED_METRICS } from '@tradelens/charting';

@Component({
  selector: 'app-company-metric-chart',
  imports: [
    NzCardComponent,
    NzIconDirective
  ],
  providers: [NzModalService],
  templateUrl: './company-metric-chart.component.html',
  styleUrl: './company-metric-chart.component.scss'
})
export class CompanyMetricChartComponent implements AfterViewInit {
  ticker: InputSignal<string | undefined> = input<string | undefined>(undefined);
  metricName: InputSignal<string | undefined> = input<string | undefined>(undefined);
  data: InputSignal<ValueDataAtEachPeriod[] | undefined> = input<ValueDataAtEachPeriod[]>();
  // new way needs to check for null data or null childMetrics depending on structure
  childMetrics: InputSignal<ChildMetricGroup[] | undefined> = input<ChildMetricGroup[]>();

  private modalService = inject(NzModalService);

  // modify later and use transformMetricName() for the inner logic here (DONE)
  protected spacedMetricName: Signal<string | undefined> = computed(() => {
    return transformMetricName(this.metricName()!);
  });

  protected chart?: Chart;

  // after component's view (template) is fully initialized and rendered into the DOM, create chart
  // or else Chart.js won't be able to find the canvas element and will throw an error
  ngAfterViewInit() {
    this.createChart();
  }

  chartChangeEffect = effect(() => {
    // listen for changes here
    this.metricName();
    this.data();
    this.childMetrics();

    // only recreate if chart already exists (means this is an update, not initial render)
    if (this.chart) {
      this.createChart();
    }
  });

  private createChart() {
    const barColor = BARCHART_COLORS[Math.floor(Math.random() * BARCHART_COLORS.length)];
    const metricNameRead = this.metricName();

    const metricDisplayName = this.spacedMetricName();
    const chartCanvasId = this.spacedMetricName();

    const dataRead = this.data();
    const childMetricsRead = this.childMetrics();

    if (!metricDisplayName || !chartCanvasId || (!dataRead && !childMetricsRead)) {
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
            },
            legend: {
              display: false
            }
          },
          // width / height
          aspectRatio: 1.2
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
            aspectRatio: 1.2
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
            aspectRatio: 1.2
          },
        });
      }
    }
  }

  protected expandCompanyMetricChart() {
    this.modalService.create({
      nzTitle: this.spacedMetricName() + ' (' + this.ticker() + ')',
      nzContent: ExpandCompanyMetricChartModalComponent,
      nzWidth: '1200px',
      nzData: {
        // parent -> modal
        tickerForModal: this.ticker(),
        metricNameForModal: this.metricName(),
        dataForModal: this.data()!,
        childMetricsForModal: this.childMetrics()!
      },
      nzFooter: null
    });
  }
}
