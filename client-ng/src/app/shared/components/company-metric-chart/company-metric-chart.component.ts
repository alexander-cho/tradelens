import { Component, effect, input, InputSignal, AfterViewInit } from '@angular/core';
import { ChildMetricGroup, ValueDataAtEachPeriod } from '../../models/fundamentals/company-fundamentals-response';
import { Chart } from 'chart.js/auto';
import { NzCardComponent } from 'ng-zorro-antd/card';

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
    const metricNameRead = this.metricName();
    const dataRead = this.data();
    const childMetricsRead = this.childMetrics();

    if (!metricNameRead || (!dataRead && !childMetricsRead)) {
      return;
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
              borderWidth: 1,
              backgroundColor: 'rgb(' + (Math.floor(Math.random() * 256)) + ',' + (Math.floor(Math.random() * 256)) + ',' + (Math.floor(Math.random() * 256)) + ')'
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
