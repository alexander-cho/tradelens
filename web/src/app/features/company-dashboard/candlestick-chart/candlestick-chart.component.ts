import { Component, inject, input, OnInit } from '@angular/core';
import { DashboardService } from '../../../core/services/dashboard.service';
import { Bar, BarAggregates } from '../../../shared/models/polygon';
import { FormsModule } from '@angular/forms';
import { ChartComponent, NgApexchartsModule } from "ng-apexcharts";
import { Stock } from "../../../shared/models/stock";
import { ChartOptions } from "../../../shared/models/charting";
import { MatCard } from "@angular/material/card";

@Component({
  selector: 'app-candlestick-chart',
  imports: [
    FormsModule, NgApexchartsModule, MatCard
  ],
  templateUrl: './candlestick-chart.component.html',
  styleUrl: './candlestick-chart.component.scss'
})
export class CandlestickChartComponent implements OnInit {
  // get ticker from parent component
  ticker = input.required<string>();

  multiplier = 5;
  timespan = "minute";
  from = "2025-08-09";
  to = "2025-08-19";
  dashboardService = inject(DashboardService);

  candlestickChartOptions = ['1d', '5d', '1m', '3m', '6m', '1y'];

  barAggregates?: BarAggregates;
  chart?: ChartComponent;
  chartOptions?: ChartOptions;

  stock?: Stock;

  constructor() {
    // initialize with empty/default values
    this.chartOptions = {
      series: [{
        name: "candle",
        data: []
      }],
      chart: {
        type: "candlestick",
        height: 350,
        width: 600
      },
      title: {
        text: "",
        align: "left"
      },
      xaxis: {
        type: "datetime"
      },
      yaxis: {
        tooltip: {
          enabled: true
        }
      }
    };
  }

  ngOnInit(): void {
    this.dashboardService.getStockByTicker(this.ticker()).subscribe({
      next: response => {
        this.stock = response;
        this.getBars();
      },
      error: err => console.log(err)
    });
  }

  onParamChange() {
    this.getBars();
  }

  getBars() {
    this.dashboardService.getBarAggregates(this.ticker(), this.multiplier, this.timespan, this.from, this.to).subscribe({
      next: response => {
        this.barAggregates = response;
        this.updateChart();
      },
      error: err => console.log(err)
    })
  }

  // convert bar aggregates data to format that chart lib expects
  convertBarsToChartData(bars?: Bar[]): any[] {
    if (!bars || bars.length === 0) {
      return [];
    }

    return bars?.map(bar => ({
      x: new Date(bar.t),
      y: [bar.o, bar.h, bar.l, bar.c]
    }));
  }

  updateChart() {
    const candleData = this.convertBarsToChartData(this.barAggregates?.results);
    this.chartOptions = {
      ...this.chartOptions,
      series: [
        {
          name: "candle",
          data: candleData
        }
      ],
      chart: {
        type: "candlestick",
        height: 350,
        width: 600
      },
      title: {
        text: this.ticker() + " " + this.multiplier + this.timespan,
        align: "left"
      },
      xaxis: {
        type: "datetime"
      },
      yaxis: {
        tooltip: {
          enabled: true
        }
      }
    }
  }
}
