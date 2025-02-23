import { Component, inject, input, OnInit } from '@angular/core';
import { DashboardService } from '../../../core/services/dashboard.service';
import { BarAggregates } from '../../../shared/models/polygon';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-candlestick-chart',
  imports: [
    FormsModule
  ],
  templateUrl: './candlestick-chart.component.html',
  styleUrl: './candlestick-chart.component.scss'
})
export class CandlestickChartComponent implements OnInit {
  // get ticker from parent component
  ticker = input.required<string>();

  multiplier = 5;
  timespan = "minute";
  from = "2025-02-13";
  to = "2025-02-14";
  dashboardService = inject(DashboardService);

  barAggregates?: BarAggregates;

  ngOnInit(): void {
    this.getBars();
  }

  onParamChange() {
    this.dashboardService.getBarAggregates(this.ticker(), this.multiplier, this.timespan, this.from, this.to).subscribe({
      next: response => {
        this.barAggregates = response;
        console.log(response);
      },
      error: err => console.log(err)
    })
  }

  getBars() {
    this.dashboardService.getBarAggregates(this.ticker(), this.multiplier, this.timespan, this.from, this.to).subscribe({
      next: response => {
        this.barAggregates = response;
        console.log(response);
      },
      error: err => console.log(err)
    })
  }
}
