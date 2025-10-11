import { AfterViewInit, Component, input, InputSignal } from '@angular/core';
import { ValueDataAtEachPeriod } from '../../models/fundamentals/company-fundamentals-response';
import { Chart } from 'chart.js/auto';

@Component({
  selector: 'app-company-metric-chart',
  imports: [],
  templateUrl: './company-metric-chart.component.html',
  styleUrl: './company-metric-chart.component.scss'
})
export class CompanyMetricChartComponent implements AfterViewInit {
  // get info necessary from parent component, e.g. company dashboard
  ticker: InputSignal<string | undefined> = input<string>();
  metricName: InputSignal<string | undefined> = input<string>();
  data: InputSignal<ValueDataAtEachPeriod[] | undefined> = input<ValueDataAtEachPeriod[]>();

  chart?: Chart;

  // after component's view (template) is fully initialized and rendered into the DOM, create chart
  // or else Chart.js won't be able to find the canvas element and will throw an error
  ngAfterViewInit() {
    if (this.metricName() || this.data()) {
      this.createChart();
    }
  }

  createChart() {
    // read both signals to track any changes
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
            label: `${ this.metricName() }`,
            data: dataRead.map(x => x.value),
            borderWidth: 1,
            backgroundColor: 'rgba(0, 250, 0, 0.7)'
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
            text: `${ this.ticker() } ${ this.metricName() }`,
            display: true
          }
        }
      },
    });
  }
}
