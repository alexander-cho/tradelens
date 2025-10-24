import { Component, effect, inject, input, OnInit, signal, WritableSignal } from '@angular/core';
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
  ticker = input.required<string>();

  multiplier = 1;
  timespan: WritableSignal<string> = signal('day');
  from: WritableSignal<string> = signal('');
  to = this.getTodayDate();
  companyDashboardService = inject(CompanyDashboardService);

  barAggregates: WritableSignal<BarAggregates | undefined> = signal(undefined);
  selectedChartOption: WritableSignal<string> = signal('1m');

  setChartOptionsEffect = effect(() => {
    if (this.selectedChartOption() == '1m') {
      this.from.set('2025-09-22');
      this.timespan.set('day');
      this.getBars();
    } else if (this.selectedChartOption() == '3m') {
      this.from.set('2025-07-22');
      this.timespan.set('day');
      this.getBars();
    } else if (this.selectedChartOption() == 'ytd') {
      this.from.set('2025-01-01');
      this.timespan.set('day');
      this.getBars();
    } else if (this.selectedChartOption() == '1y') {
      this.from.set('2024-10-22');
      this.timespan.set('week');
      this.getBars();
    } else if (this.selectedChartOption() == '5y') {
      this.from.set('2020-10-22');
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

  createChart() {
    const barAggregatesRead = this.barAggregates();
    if (!barAggregatesRead) {
      return;
    }

    console.log(barAggregatesRead);

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

    this.chart = new Chart('stockPriceChart', {
      type: 'line',
      data: {
        labels: labels,
        datasets: [
          {
            data: barAggregatesRead.results.map(x => x.c),
            borderWidth: 3,
            backgroundColor: 'rgba(0, 250, 0, 0.7)',
            borderColor: 'rgba(0, 250, 0, 0.7)',
            pointRadius: 0
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
