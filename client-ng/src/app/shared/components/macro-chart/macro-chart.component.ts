import { AfterViewInit, Component, effect, input, InputSignal, OnInit } from '@angular/core';
import { Chart } from 'chart.js/auto';
import { FredDataPoint } from '../../models/macro';

@Component({
  selector: 'app-macro-chart',
  imports: [

  ],
  templateUrl: './macro-chart.component.html',
  styleUrl: './macro-chart.component.scss'
})
export class MacroChartComponent implements AfterViewInit {

  data: InputSignal<FredDataPoint[] | undefined> = input<FredDataPoint[]>();
  chart?: Chart;

  ngAfterViewInit() {
    this.createMacroChart();
    console.log(this.data());
  }

  chartChangeEffect = effect(() => {
    // listen for changes here
    this.data();

    // only recreate if chart already exists (means this is an update, not initial render)
    if (this.chart) {
      this.createMacroChart();
    }
  });

  createMacroChart() {
    const dataRead = this.data();

    if (!dataRead) {
      return;
    }

    this.chart?.destroy();

    this.chart = new Chart('marginChart', {
      type: 'line',
      data: {
        labels: dataRead.map(x => x.date),
        datasets: [
          {
            label: 'margin',
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
