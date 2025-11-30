import { Component, input, InputSignal, OnInit } from '@angular/core';
import { Chart } from 'chart.js/auto';
import { ValueDataAtEachPeriod } from '../../models/fundamentals/company-fundamentals-response';

@Component({
  selector: 'app-macro-chart',
  imports: [],
  templateUrl: './macro-chart.component.html',
  styleUrl: './macro-chart.component.scss'
})
export class MacroChartComponent implements OnInit {

  data: InputSignal<string[] | undefined> = input<string[]>();
  chart?: Chart;

  ngOnInit() {
    this.createOiVolChart();
  }

  createOiVolChart() {
    // const metricChoiceRead = this.metricChoice();
    // const metricDataRead = this.metricData();
    //
    // if (!metricDataRead || !metricChoiceRead) {
    //   return;
    // }

    this.chart?.destroy();

    this.chart = new Chart('marginChart', {
      type: 'line',
      data: {
        labels: ['1', '2', '3' ,'4'],
        datasets: [
          {
            label: 'margin',
            data: [12341234, 23452345, 34563456, 45674567],
            borderWidth: 1,
            backgroundColor: 'rgba(0, 250, 0, 0.7)'
          }
        ],
      },
      options: {
        scales: {
          x: {},
          y: {
            beginAtZero: true,
          },
        },
        plugins: {
          title: {
            text: `margin lol`,
            display: true
          }
        }
      }
    });
  }
}
