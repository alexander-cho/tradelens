import { Component, effect, input, InputSignal } from '@angular/core';
import { OptionMetricData } from '../../models/options/options-data-shapes';
import { Chart } from 'chart.js/auto';

@Component({
  selector: 'app-options-metric-chart',
  imports: [
  ],
  templateUrl: './options-metric-chart.component.html',
  styleUrl: './options-metric-chart.component.scss'
})
export class OptionsMetricChartComponent {
  // get specifics so we know what kind of chart, for which options metric to display/render
  metricChoice: InputSignal<string | undefined> = input<string>();
  metricData: InputSignal<OptionMetricData[] | undefined> = input<OptionMetricData[] | undefined>(undefined);

  chart?: Chart;

  chartChangeEffect = effect(() => {
    const metricChoiceRead = this.metricChoice();

    if (!this.chart || metricChoiceRead === 'openInterest' || metricChoiceRead === 'volume') {
      this.createVolOiChart();
    } else {
      console.log('cannot render chart');
    }
  });

  createVolOiChart() {
    const metricChoiceRead = this.metricChoice();
    const metricDataRead = this.metricData();

    if (!metricDataRead || !metricChoiceRead || (typeof metricDataRead.map(x => x.data) === 'number')) {
      return;
    }

    this.chart?.destroy();

    this.chart = new Chart("optionMetric", {
      type: 'bar',
      data: {
        labels: metricDataRead.filter(x => x.optionType === 'call').map(x => x.strike),
        datasets: [
          {
            label: 'Calls',
            data: metricDataRead.filter(x => x.optionType === 'call').map(x => x.data as number),
            borderWidth: 1,
            backgroundColor: 'rgba(0, 250, 0, 0.7)'
          },
          {
            label: 'Puts',
            data: metricDataRead.filter(x => x.optionType === 'put').map(x => x.data as number),
            borderWidth: 1,
            backgroundColor: 'rgba(250, 0, 0, 0.7)'
          }
        ],
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
            text: `${ this.metricChoice() }`,
            display: true
          }
        }
      }
    });
  }
}
