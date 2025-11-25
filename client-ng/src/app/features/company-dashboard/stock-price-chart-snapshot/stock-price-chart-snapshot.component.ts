import { Component, effect, inject, input, InputSignal, OnInit, signal, WritableSignal } from '@angular/core';
import { Stock } from '../../../shared/models/stock';
import { CompanyDashboardService } from '../../../core/services/company-dashboard.service';
import { BarAggregates } from '../../../shared/models/polygon';
import { Chart } from 'chart.js/auto';
import { NzCardComponent } from 'ng-zorro-antd/card';
import { NzRadioComponent, NzRadioGroupComponent } from 'ng-zorro-antd/radio';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-stock-price-chart-snapshot',
  imports: [
    NzCardComponent,
    NzRadioComponent,
    NzRadioGroupComponent,
    FormsModule
  ],
  templateUrl: './stock-price-chart-snapshot.component.html',
  styleUrl: './stock-price-chart-snapshot.component.scss'
})
export class StockPriceChartSnapshotComponent implements OnInit {
  // get ticker from parent component
  ticker: InputSignal<string> = input.required<string>();

  multiplier = 1;
  timespan: WritableSignal<string> = signal('day');
  from: WritableSignal<string> = signal('');
  to = this.getTodayDate();
  companyDashboardService = inject(CompanyDashboardService);

  barAggregates: WritableSignal<BarAggregates | undefined> = signal(undefined);
  selectedChartOption: WritableSignal<string> = signal('1m');

  setChartOptionsEffect = effect(() => {
    if (this.selectedChartOption() == '1m') {
      this.from.set('2025-10-25');
      this.timespan.set('day');
      this.getBars();
    } else if (this.selectedChartOption() == '3m') {
      this.from.set('2025-08-25');
      this.timespan.set('day');
      this.getBars();
    } else if (this.selectedChartOption() == 'ytd') {
      this.from.set('2025-01-01');
      this.timespan.set('day');
      this.getBars();
    } else if (this.selectedChartOption() == '1y') {
      this.from.set('2024-11-25');
      this.timespan.set('week');
      this.getBars();
    } else if (this.selectedChartOption() == '5y') {
      this.from.set('2020-11-25');
      this.timespan.set('week');
      this.getBars();
    }
  })

  stock?: Stock;
  chart?: Chart;

  ngOnInit(): void {
    this.companyDashboardService.getStockByTicker(this.ticker()).subscribe({
      next: response => {
        this.stock = response;
        this.getBars();
      },
      error: err => console.log(err)
    });
  }

  getBars() {
    this.companyDashboardService.getBarAggregates(this.ticker(), this.multiplier, this.timespan(), this.from(), this.to).subscribe({
      next: response => {
        this.barAggregates?.set(response);
        this.createChart();
      },
      error: err => console.log(err)
    })
  }

  // have this in a separate utils maybe
  getTodayDate() {
    let today = new Date();
    let dd = today.getDate();
    let mm = today.getMonth() + 1;
    let yyyy = today.getFullYear();

    let ddStr = dd < 10 ? '0' + dd : dd.toString();
    let mmStr = mm < 10 ? '0' + mm : mm.toString();

    return yyyy + '-' + mmStr + '-' + ddStr;
  }

  isPriceUp(): boolean {
    const aggregates = this.barAggregates();
    if (aggregates?.results && aggregates.results.length > 1) {
      return aggregates.results[0].c < aggregates.results[aggregates.results.length - 1].c;
    }
    return false;
  }

  createChart() {
    const barAggregatesRead = this.barAggregates();
    if (!barAggregatesRead) {
      return;
    }

    const isPositive = this.isPriceUp();
    const color = isPositive ? 'rgba(0, 250, 0, 0.7)' : 'rgba(250, 0, 0, 0.7)';

    this.chart?.destroy();

    // convert timestamps based on selected option for x-axis
    const labels = barAggregatesRead.results.map(x => {
      const date = new Date(x.t);
      const option = this.selectedChartOption();

      if (option === '1m' || option === '3m') {
        return date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });
      } else if (option === 'ytd' || option === '1y' || option === '5y') {
        return date.toLocaleDateString('en-US', { month: 'short' });
      }
      return '';
    });

    const createGradient = (ctx: any, chartArea: any, isPositive: boolean) => {
      const gradient = ctx.createLinearGradient(0, chartArea.bottom, 0, chartArea.top);

      const bottomColor = 'rgba(37, 42, 65, 0.4)'; // very subtle (almost transparent)

      const topColorGreen = 'rgba(0, 250, 0, 0.7)'; // lightly subtle opacity green
      const topColorRed = 'rgba(250, 0, 0, 0.7)'; // lightly subtle opacity red

      const topColor = isPositive ? topColorGreen : topColorRed;

      // color stops for the gradient
      gradient.addColorStop(0, bottomColor); // start (bottom) is transparent/dark
      gradient.addColorStop(1, topColor);    // end (top) is semi-transparent green or red

      return gradient;
    };

    this.chart = new Chart('stockPriceChart', {
      type: 'line',
      data: {
        labels: labels,
        datasets: [
          {
            data: barAggregatesRead.results.map(x => x.c),
            borderWidth: 2,
            // 1. Set the background color to the gradient function
            backgroundColor: function (context) {
              const chart = context.chart;
              const { ctx, chartArea } = chart;
              if (!chartArea) {
                return;
              }
              return createGradient(ctx, chartArea, isPositive);
            },
            borderColor: color,
            pointRadius: 0,
            fill: 'origin'
          }
        ],
      },
      options: {
        maintainAspectRatio: false,
        scales: {
          x: {
            ticks: {
              maxTicksLimit: 8,  // limit number of labels shown
              autoSkip: true,     // skip labels automatically
            }
          },
          y: {
            beginAtZero: false
          },
        },
        plugins: {
          title: {
            display: false
          },
          legend: {
            display: false
          }
        }
      }
    });
  }
}
