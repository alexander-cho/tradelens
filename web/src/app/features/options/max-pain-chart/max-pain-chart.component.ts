import { Component, input, InputSignal, OnInit } from '@angular/core';
import { Chart } from 'chart.js/auto';
import { CallsAndPutsCashSums } from '../../../shared/models/options';

@Component({
  selector: 'app-max-pain-chart',
  imports: [],
  templateUrl: './max-pain-chart.component.html',
  styleUrl: './max-pain-chart.component.scss'
})
export class MaxPainChartComponent implements OnInit {
  cashValuesDataForChart: InputSignal<CallsAndPutsCashSums | undefined> = input<CallsAndPutsCashSums>();
  expirationDate: InputSignal<string | undefined> = input<string>();

  chart: any = [];

  ngOnInit() {
    this.createMaxPainChart();
  }

  createMaxPainChart() {
    if (this.cashValuesDataForChart) {
      console.log(this.cashValuesDataForChart());
      this.chart = new Chart('maxPainChart', {
        type: 'bar',
        data: {
          labels: this.cashValuesDataForChart()?.callCashSums.map(x => x.price),
          datasets: [
            {
              label: 'Calls',
              data: this.cashValuesDataForChart()?.callCashSums.map(x => x.totalCashValue),
              borderWidth: 1,
              backgroundColor: 'rgba(0, 250, 0, 0.7)'
            },
            {
              label: 'Puts',
              data: this.cashValuesDataForChart()?.putCashSums.map(x => x.totalCashValue),
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
              text: 'Cash Values for <ticker> expiring <date>',
              display: true
            }
          }
        },
      });
    }
  }
}
