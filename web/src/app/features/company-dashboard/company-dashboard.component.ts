import { Component, inject, input, OnInit } from '@angular/core';
import { Stock } from "../../shared/models/stock";
import { RelatedCompanies } from '../../shared/models/polygon';
// import { CandlestickChartComponent } from './candlestick-chart/candlestick-chart.component';
import { CompanyDashboardService } from '../../core/services/company-dashboard.service';
import { IncomeStatement } from '../../shared/models/fundamentals/incomeStatement';

@Component({
  selector: 'app-company-dashboard',
  imports: [
    // CandlestickChartComponent
  ],
  templateUrl: './company-dashboard.component.html',
  styleUrl: './company-dashboard.component.scss'
})
export class CompanyDashboardComponent implements OnInit {
  // get ticker from url path
  ticker = input.required<string>();
  companyDashboardService = inject(CompanyDashboardService);
  stock?: Stock;
  relatedCompanies?: RelatedCompanies;

  period = "quarter";
  incomeStatement?: IncomeStatement;

  ngOnInit() {
    this.companyDashboardService.getStockByTicker(this.ticker()).subscribe({
      next: response => {
        this.stock = response;
        this.getRelatedCompanies();
        this.getIncomeStatement();
      },
      error: err => console.log(err)
    });
  }

  getRelatedCompanies() {
    if (this.stock) {
      this.companyDashboardService.getRelatedCompanies(this.stock.ticker).subscribe({
        next: response => {
          this.relatedCompanies = response;
          console.log(response);
        },
        error: err => console.log(err)
      });
    }
  }

  getIncomeStatement() {
    if (this.stock) {
      this.companyDashboardService.getIncomeStatement(this.stock.ticker, this.period).subscribe({
        next: response => {
          this.incomeStatement = response;
          console.log(response);
        },
        error: err => console.log(err)
      })
    }
  }
}
