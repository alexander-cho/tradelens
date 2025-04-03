import { Component, inject, input, OnInit } from '@angular/core';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { DashboardService } from '../../core/services/dashboard.service';
import { RelatedCompanies } from '../../shared/models/polygon';
import { CandlestickChartComponent } from './candlestick-chart/candlestick-chart.component';
// import { RouterLink } from '@angular/router';
import { Stock } from "../../shared/models/stock";

@Component({
  selector: 'app-company-dashboard',
  imports: [
    NavbarComponent,
    CandlestickChartComponent,
    // RouterLink
  ],
  templateUrl: './company-dashboard.component.html',
  styleUrl: './company-dashboard.component.scss'
})
export class CompanyDashboardComponent implements OnInit {
  // get ticker from url path
  ticker = input.required<string>();
  dashboardService = inject(DashboardService);
  stock?: Stock;
  relatedCompanies?: RelatedCompanies;

  ngOnInit() {
    this.dashboardService.getStockByTicker(this.ticker()).subscribe({
      next: response => {
        this.stock = response;
        this.getRelatedCompanies();
      },
      error: err => console.log(err)
    });
  }

  getRelatedCompanies() {
    if (this.stock) {
      this.dashboardService.getRelatedCompanies(this.stock.ticker).subscribe({
        next: response => {
          this.relatedCompanies = response;
          console.log(response);
        },
        error: err => console.log(err)
      })
    }
  }
}
