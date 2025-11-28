import { Component, effect, input, InputSignal } from '@angular/core';
import { Chart } from 'chart.js/auto';

@Component({
  selector: 'app-chart-engine-chart',
  imports: [],
  templateUrl: './chart-engine-chart.component.html',
  styleUrl: './chart-engine-chart.component.scss'
})
export class ChartEngineChartComponent {
  titleFromParent: InputSignal<string | undefined> = input<string>();
  xAxisDataLabelsFromParent: InputSignal<string[] | undefined> = input<string[]>();
  yDataFromParent: InputSignal<number[] | undefined> = input<number[]>();
  yLabelFromParent: InputSignal<string | undefined> = input<string>();
  // chartType: string[] = ['bar', 'line', 'scatter'];

  chart?: Chart;

  chartEffect = effect(() => {
    this.createChart();
  });

  createChart() {
    this.chart?.destroy();

    this.chart = new Chart("adhoc-chart", {
      type: 'line',
      data: {
        labels: this.xAxisDataLabelsFromParent(),
        datasets: [
          {
            label: this.yLabelFromParent(),
            data: this.yDataFromParent() as number[],
            // borderWidth: 1,
            borderColor: '#DB2504',
            backgroundColor: '#DB2504',
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
            text: this.titleFromParent(),
            display: true
          }
        }
      },
    });
  }

  destroyChart() {
    this.chart?.destroy();
  }
}
