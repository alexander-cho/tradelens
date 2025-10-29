import { Component, effect, input, InputSignal } from '@angular/core';
import { Chart } from 'chart.js/auto';
import { CallsAndPutsCashSums } from '../../../shared/models/options';

@Component({
  selector: 'app-max-pain-chart',
  imports: [
  ],
  templateUrl: './max-pain-chart.component.html',
  styleUrl: './max-pain-chart.component.scss'
})
export class MaxPainChartComponent {
  tickerSymbol: InputSignal<string | undefined> = input<string>();
  cashValuesDataForChart: InputSignal<CallsAndPutsCashSums | undefined> = input<CallsAndPutsCashSums>();
  expirationDate: InputSignal<string | undefined> = input<string>();

  chart?: Chart;

  chartChangeEffect = effect(() => {
    if (!this.chart) {
      this.createChart();
    } else {
      this.updateChart(this.cashValuesDataForChart(), this.expirationDate());
    }
  });

  createChart() {
    // read both signals to track changes
    const newCashData = this.cashValuesDataForChart();
    const newExpiryDate = this.expirationDate();

    if (!newCashData || !newCashData.callCashSums || !newCashData.putCashSums) {
      return;
    }

    this.chart?.destroy();

    this.chart = new Chart('maxPainChart', {
      type: 'bar',
      data: {
        labels: newCashData?.callCashSums.map(x => x.price),
        datasets: [
          {
            label: 'Calls',
            data: newCashData?.callCashSums.map(x => x.totalCashValue),
            borderWidth: 1,
            backgroundColor: 'rgba(0, 250, 0, 0.7)'
          },
          {
            label: 'Puts',
            data: newCashData?.putCashSums.map(x => x.totalCashValue),
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
            text: `Cash Values for ${ this.tickerSymbol()?.toUpperCase() } expiring ${ newExpiryDate }. Max Pain lies at ${ newCashData.maxPainValue }`,
            display: true
          }
        },
        interaction: {
          mode: 'index',
        },
      }
    });
  }

  updateChart(newCashData: CallsAndPutsCashSums | undefined, newExpiryDate: string | undefined) {
    if (!newCashData || !newCashData.callCashSums || !newCashData.putCashSums) {
      return;
    }

    if (this.chart && this.chart.options.plugins?.title?.text) {
      this.chart.data.labels = newCashData?.callCashSums.map(x => x.price);
      this.chart.data.datasets[0].data = newCashData?.callCashSums.map(x => x.totalCashValue);
      this.chart.data.datasets[1].data = newCashData?.putCashSums.map(x => x.totalCashValue);
      this.chart.options.plugins.title.text = `Cash Values for ${ this.tickerSymbol()?.toUpperCase() ?? 'Unknown' } expiring ${ newExpiryDate }. Max Pain lies at ${ newCashData.maxPainValue }`;
    }

    this.chart?.update();
  }
}
