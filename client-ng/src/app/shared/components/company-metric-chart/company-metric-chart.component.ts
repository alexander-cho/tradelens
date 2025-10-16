import { Component, effect, input, InputSignal, AfterViewInit } from '@angular/core';
import { ValueDataAtEachPeriod } from '../../models/fundamentals/company-fundamentals-response';
import { Chart } from 'chart.js/auto';

@Component({
  selector: 'app-company-metric-chart',
  imports: [],
  templateUrl: './company-metric-chart.component.html',
  styleUrl: './company-metric-chart.component.scss'
})
export class CompanyMetricChartComponent implements AfterViewInit {
  ticker: InputSignal<string | undefined> = input<string>();
  metricName: InputSignal<string | undefined> = input<string>();
  data: InputSignal<ValueDataAtEachPeriod[] | undefined> = input<ValueDataAtEachPeriod[]>();

  chart?: Chart;

  // after component's view (template) is fully initialized and rendered into the DOM, create chart
  // or else Chart.js won't be able to find the canvas element and will throw an error
  ngAfterViewInit() {
    this.createChart();
  }

  chartChangeEffect = effect(() => {
    const metricNameRead = this.metricName();
    const dataRead = this.data();

    if (!metricNameRead || !dataRead) {
      return;
    }

    // only recreate if chart already exists (means this is an update, not initial render)
    if (this.chart) {
      this.createChart();
    }
  });

  createChart() {
    const metricNameRead = this.metricName();
    const dataRead = this.data();

    if (!metricNameRead || !dataRead) {
      return;
    }

    this.chart?.destroy();

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
            display: true
          }
        }
      },
    });
  }
}
