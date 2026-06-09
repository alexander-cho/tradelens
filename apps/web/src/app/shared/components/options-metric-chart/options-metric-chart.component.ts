import { Component, effect, input, InputSignal, signal, WritableSignal } from '@angular/core';
import { OptionMetricData } from '../../models/options/options-data-shapes';
import { Chart } from 'chart.js/auto';
import { Activity, Greeks, ImpliedVolatility } from '../../models/options/options-chain';
import { NzRadioComponent, NzRadioGroupComponent } from 'ng-zorro-antd/radio';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-options-metric-chart',
  imports: [
    NzRadioComponent,
    NzRadioGroupComponent,
    FormsModule
  ],
  templateUrl: './options-metric-chart.component.html',
  styleUrl: './options-metric-chart.component.scss'
})
export class OptionsMetricChartComponent {
  // get specifics so we know what kind of chart, for which options metric to display/render
  metricChoice: InputSignal<string | undefined> = input<string>();
  metricData: InputSignal<OptionMetricData[] | undefined> = input<OptionMetricData[] | undefined>(undefined);

  // users may view by greek
  selectedGreek: WritableSignal<string> = signal('delta');
  selectedUnusualCallOrPut: WritableSignal<string> = signal('call');
  selectedIvCallOrPut: WritableSignal<string> = signal('call')

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

    this.chart = new Chart('metricChart', {
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
        },
        // tooltip should show all dataset values at that index
        interaction: {
          mode: 'index',
        },
      }
    });
  }

  createGreeksChart() {
    // read which greek user selected
    const selectedGreekRead = this.selectedGreek();

    const metricChoiceRead = this.metricChoice();
    const metricDataRead = this.metricData();

    if (!metricDataRead || !metricChoiceRead) {
      return;
    }

    this.chart?.destroy();

    this.chart = new Chart('metricChart', {
      type: 'line',
      data: {
        labels: metricDataRead.filter(x => x.optionType === 'call').map(x => x.strike),
        datasets: [
          {
            label: `${ selectedGreekRead } (Calls)`,
            // cast as valid key in Greeks object type, and cast as number since it has both number and string types, .e.g. updatedAt
            data: metricDataRead.filter(x => x.optionType === 'call').map(x => (x.data as Greeks)[selectedGreekRead as keyof Greeks] as number),
            borderColor: 'rgba(0, 250, 0, 0.7)',
            backgroundColor: 'rgba(0, 250, 0, 0.7)'
          },
          {
            label: `${ selectedGreekRead } (Puts)`,
            data: metricDataRead.filter(x => x.optionType === 'put').map(x => (x.data as Greeks)[selectedGreekRead as keyof Greeks] as number),
            borderColor: 'rgba(250, 0, 0, 0.7)',
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
        },
        interaction: {
          mode: 'index',
        },
      }
    });
  }

  createIvChart() {
    const selectedIvCallOrPutRead = this.selectedIvCallOrPut();
    const metricChoiceRead = this.metricChoice();
    const metricDataRead = this.metricData();

    const color = selectedIvCallOrPutRead === 'call' ? 'rgba(0, 250, 0, 0.7)' : 'rgba(250, 0, 0, 0.7)';

    if (!metricDataRead || !metricChoiceRead) {
      return;
    }

    this.chart?.destroy();

    this.chart = new Chart('metricChart', {
      type: 'line',
      data: {
        labels: metricDataRead.filter(x => x.optionType === selectedIvCallOrPutRead).map(x => x.strike),
        datasets: [
          {
            label: `Bid IV (${ selectedIvCallOrPutRead })`,
            data: metricDataRead.filter(x => x.optionType === selectedIvCallOrPutRead).map(x => (x.data as ImpliedVolatility).bidIv),
            borderColor: color,
            backgroundColor: color
          },
          {
            label: `Ask IV (${ selectedIvCallOrPutRead })`,
            data: metricDataRead.filter(x => x.optionType === selectedIvCallOrPutRead).map(x => (x.data as ImpliedVolatility).askIv),
            borderColor: color,
            backgroundColor: color
          },
          {
            label: `Mid IV (${ selectedIvCallOrPutRead })`,
            data: metricDataRead.filter(x => x.optionType === selectedIvCallOrPutRead).map(x => (x.data as ImpliedVolatility).midIv),
            borderColor: color,
            backgroundColor: color
          },
          {
            label: `SMV (${ selectedIvCallOrPutRead })`,
            data: metricDataRead.filter(x => x.optionType === selectedIvCallOrPutRead).map(x => (x.data as ImpliedVolatility).smvVol),
            borderColor: 'rgba(255, 255, 255, 1)',
            backgroundColor: 'rgba(255, 255, 255, 1)'
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
        },
        // tooltip should show all dataset values at that index
        interaction: {
          mode: 'index',
        },
      }
    });
  }

  createUnusualActivityChart() {
    const selectedUnusualCallOrPutRead = this.selectedUnusualCallOrPut();
    const metricChoiceRead = this.metricChoice();
    const metricDataRead = this.metricData();

    // set color of bars based on user choice between call or put
    const colors: string[] = selectedUnusualCallOrPutRead === 'call' ? ['rgba(144, 255, 144, 0.7)', 'rgba(0, 180, 0, 0.7)'] : ['rgba(255, 160, 160, 0.7)', 'rgba(250, 0, 0, 0.7)'];

    if (!metricDataRead || !metricChoiceRead) {
      return;
    }

    this.chart?.destroy();

    this.chart = new Chart("metricChart", {
      type: 'bar',
      data: {
        labels: metricDataRead.filter(x => x.optionType === selectedUnusualCallOrPutRead).map(x => x.strike),
        datasets: [
          {
            label: `Volume/OI Ratio (${ selectedUnusualCallOrPutRead })`,
            data: metricDataRead.filter(x => x.optionType === selectedUnusualCallOrPutRead).map(x => (x.data as Activity).unusualActivity),
            borderColor: 'rgba(255, 255, 255, 1)',
            backgroundColor: 'rgba(255, 255, 255, 1)',
            type: 'line',
            yAxisID: 'y'
          },
          {
            label: `Volume (${ selectedUnusualCallOrPutRead })`,
            data: metricDataRead.filter(x => x.optionType === selectedUnusualCallOrPutRead).map(x => (x.data as Activity).volume),
            borderColor: colors[1],
            backgroundColor: colors[1],
            yAxisID: 'y1'
          },
          {
            label: `Open Interest (${ selectedUnusualCallOrPutRead })`,
            data: metricDataRead.filter(x => x.optionType === selectedUnusualCallOrPutRead).map(x => (x.data as Activity).openInterest),
            borderColor: colors[0],
            backgroundColor: colors[0],
            yAxisID: 'y1'
          }
        ],
      },
      options: {
        scales: {
          x: {},
          y: {
            beginAtZero: true,
            position: 'left'
          },
          y1: {
            beginAtZero: true,
            position: 'right',

            // grid line settings
            grid: {
              drawOnChartArea: false, // only want the grid lines for one axis to show up
            },
          },
        },
        // tooltip should show all dataset values at that index
        interaction: {
          mode: 'index',
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
