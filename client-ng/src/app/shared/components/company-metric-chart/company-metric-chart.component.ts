import { Component, effect, input, InputSignal, AfterViewInit } from '@angular/core';
import { ChildMetricGroup, ValueDataAtEachPeriod } from '../../models/fundamentals/company-fundamentals-response';
import { Chart, ScriptableContext } from 'chart.js/auto';
import { NzCardComponent } from 'ng-zorro-antd/card';
import { barchartColors } from '../../utils/barchart-colors';

@Component({
  selector: 'app-company-metric-chart',
  imports: [
    NzCardComponent
  ],
  templateUrl: './company-metric-chart.component.html',
  styleUrl: './company-metric-chart.component.scss'
})
export class CompanyMetricChartComponent implements AfterViewInit {
  ticker: InputSignal<string | undefined> = input<string>();
  metricName: InputSignal<string | undefined> = input<string>();
  data: InputSignal<ValueDataAtEachPeriod[] | undefined> = input<ValueDataAtEachPeriod[]>();

  // new way needs a way for null data or null childMetrics depending on structure
  childMetrics: InputSignal<ChildMetricGroup[] | undefined> = input<ChildMetricGroup[]>();

  chart?: Chart;

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

  createChart() {
    const barColor = barchartColors[Math.floor(Math.random() * barchartColors.length)];
    const metricNameRead = this.metricName();
    const dataRead = this.data();
    const childMetricsRead = this.childMetrics();

    if (!metricNameRead || (!dataRead && !childMetricsRead)) {
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

    if (dataRead != null) {
      this.chart = new Chart(metricNameRead, {
        type: 'bar',
        data: {
          labels: dataRead.map(x => x.period + ' ' + x.fiscalYear),
          datasets: [
            {
              label: metricNameRead,
              data: dataRead.map(x => x.value),
              backgroundColor: function(context: ScriptableContext<'bar'>) {
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
          aspectRatio: 1.3
        },
      });
    } else if (childMetricsRead != null && metricNameRead != 'EPS') {
      this.chart = new Chart(metricNameRead, {
        type: 'bar',
        data: {
          labels: childMetricsRead[0].data.map(x => x.period + ' ' + x.fiscalYear),
          datasets: childMetricsRead.map((childMetric, index) => ({
            label: childMetric.metricName,
            data: childMetric.data.map(x => x.value),
            backgroundColor: function(context: ScriptableContext<'bar'>) {
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
          aspectRatio: 1.3
        },
      });
    } else if (childMetricsRead != null && metricNameRead == 'EPS') {
      this.chart = new Chart(metricNameRead, {
        type: 'bar',
        data: {
          labels: childMetricsRead[0].data.map(x => x.period + ' ' + x.fiscalYear),
          // datasets: [
          //   {
          //     label: childMetricsRead[0].metricName,
          //     data: childMetricsRead[0].data.map(x => x.value),
          //     borderWidth: 1,
          //     backgroundColor: 'rgb(' + (Math.floor(Math.random() * 256)) + ',' + (Math.floor(Math.random() * 256)) + ',' + (Math.floor(Math.random() * 256)) + ')'
          //   }
          // ],
          datasets: childMetricsRead.map(childMetric => ({
            label: childMetric.metricName,
            data: childMetric.data.map(x => x.value),
            borderWidth: 1,
            backgroundColor: 'rgb(' + (Math.floor(Math.random() * 256)) + ',' + (Math.floor(Math.random() * 256)) + ',' + (Math.floor(Math.random() * 256)) + ')'
          }))
        },
        options: {
          scales: {
            x: {
            },
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
          aspectRatio: 1.3
        },
      });
    }
  }
}
