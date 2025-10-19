import { Component, effect, input, InputSignal } from '@angular/core';
import { OptionMetricData } from '../../models/options/options-data-shapes';
import { Chart } from 'chart.js/auto';
import { Activity, Greeks, ImpliedVolatility } from '../../models/options/options-chain';

@Component({
  selector: 'app-options-metric-chart',
  imports: [],
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

    if (metricChoiceRead === 'openInterest' || metricChoiceRead === 'volume') {
      this.createOiVolChart();
    } else if (metricChoiceRead === 'greeks') {
      this.createGreeksChart();
    } else if (metricChoiceRead === 'impliedVolatility') {
        this.createIvChart();
    } else if (metricChoiceRead === 'activity') {
      this.createUnusualActivityChart();
    }
  });

  createOiVolChart() {
    const metricChoiceRead = this.metricChoice();
    const metricDataRead = this.metricData();

    if (!metricDataRead || !metricChoiceRead) {
      return;
    }

    this.chart?.destroy();

    this.chart = new Chart("metricChart", {
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

  createGreeksChart() {
    const metricChoiceRead = this.metricChoice();
    const metricDataRead = this.metricData();

    if (!metricDataRead || !metricChoiceRead) {
      return;
    }

    this.chart?.destroy();

    this.chart = new Chart("metricChart", {
      type: 'line',
      data: {
        labels: metricDataRead.filter(x => x.optionType === 'call').map(x => x.strike),
        datasets: [
          {
            label: 'Delta (Calls)',
            data: metricDataRead.filter(x => x.optionType === 'call').map(x => (x.data as Greeks).delta),
            borderWidth: 1,
            backgroundColor: 'rgba(0, 250, 0, 0.7)'
          }
        ],
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
            text: `${ this.metricChoice() }`,
            display: true
          }
        }
      }
    });
  }

  createIvChart() {
    const metricChoiceRead = this.metricChoice();
    const metricDataRead = this.metricData();

    if (!metricDataRead || !metricChoiceRead) {
      return;
    }

    this.chart?.destroy();

    this.chart = new Chart("metricChart", {
      type: 'line',
      data: {
        labels: metricDataRead.filter(x => x.optionType === 'call').map(x => x.strike),
        datasets: [
          {
            label: 'Call Bid IV',
            data: metricDataRead.filter(x => x.optionType === 'call').map(x => (x.data as ImpliedVolatility).bidIv),
            borderWidth: 1,
            backgroundColor: 'rgba(0, 250, 0, 0.7)'
          },
          {
            label: 'Call Ask IV',
            data: metricDataRead.filter(x => x.optionType === 'call').map(x => (x.data as ImpliedVolatility).askIv),
            borderWidth: 1,
            backgroundColor: 'rgba(250, 0, 0, 0.7)'
          },
          {
            label: 'Call Mid IV',
            data: metricDataRead.filter(x => x.optionType === 'call').map(x => (x.data as ImpliedVolatility).midIv),
            borderWidth: 1,
            backgroundColor: 'rgba(250, 0, 0, 0.7)'
          },
          {
            label: 'Call SMV',
            data: metricDataRead.filter(x => x.optionType === 'call').map(x => (x.data as ImpliedVolatility).smvVol),
            borderWidth: 1,
            backgroundColor: 'rgba(250, 0, 0, 0.7)'
          },
          {
            label: 'Put Bid IV',
            data: metricDataRead.filter(x => x.optionType === 'put').map(x => (x.data as ImpliedVolatility).bidIv),
            borderWidth: 1,
            backgroundColor: 'rgba(250, 0, 0, 0.7)'
          },
          {
            label: 'Put Mid IV',
            data: metricDataRead.filter(x => x.optionType === 'put').map(x => (x.data as ImpliedVolatility).askIv),
            borderWidth: 1,
            backgroundColor: 'rgba(250, 0, 0, 0.7)'
          },
          {
            label: 'Put Mid IV',
            data: metricDataRead.filter(x => x.optionType === 'put').map(x => (x.data as ImpliedVolatility).midIv),
            borderWidth: 1,
            backgroundColor: 'rgba(250, 0, 0, 0.7)'
          },
          {
            label: 'Put SMV',
            data: metricDataRead.filter(x => x.optionType === 'put').map(x => (x.data as ImpliedVolatility).smvVol),
            borderWidth: 1,
            backgroundColor: 'rgba(250, 0, 0, 0.7)'
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
            text: `${ this.metricChoice() }`,
            display: true
          }
        }
      }
    });
  }

  createUnusualActivityChart() {
    const metricChoiceRead = this.metricChoice();
    const metricDataRead = this.metricData();

    if (!metricDataRead || !metricChoiceRead) {
      return;
    }

    this.chart?.destroy();

    this.chart = new Chart("metricChart", {
      type: 'line',
      data: {
        labels: metricDataRead.filter(x => x.optionType === 'call').map(x => x.strike),
        datasets: [
          {
            label: 'Volume/OI Ratio: Calls',
            data: metricDataRead.filter(x => x.optionType === 'call').map(x => (x.data as Activity).unusualActivity),
            borderColor: 'rgba(0, 250, 0, 0.7)',
            backgroundColor: 'rgba(0, 250, 0, 0.7)'
          }
        ],
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
            text: `${ this.metricChoice() }`,
            display: true
          }
        }
      }
    });
  }
}
