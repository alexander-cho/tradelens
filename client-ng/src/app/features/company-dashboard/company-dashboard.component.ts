import { Component, inject, input, OnInit, signal, WritableSignal } from '@angular/core';
import { Stock } from "../../shared/models/stock";
import { RelatedCompanies } from '../../shared/models/polygon';
// import { CandlestickChartComponent } from './candlestick-chart/candlestick-chart.component';
import { CompanyDashboardService } from '../../core/services/company-dashboard.service';
import { IncomeStatement } from '../../shared/models/fundamentals/income-statement';
import { Router } from '@angular/router';

@Component({
  selector: 'app-company-dashboard',
  imports: [
    // CandlestickChartComponent
  ],
  templateUrl: './company-dashboard.component.html',
  styleUrl: './company-dashboard.component.scss'
})
export class CompanyDashboardComponent implements OnInit {
  // get ticker from url path, as defined in routes
  ticker = input.required<string>();

  companyDashboardService = inject(CompanyDashboardService);
  router = inject(Router);

  relatedCompanies: WritableSignal<RelatedCompanies | undefined> = signal(undefined);
  incomeStatement: WritableSignal<IncomeStatement | undefined> = signal(undefined);
  stock: WritableSignal<Stock | undefined> = signal(undefined);

  period = "quarter";

  ngOnInit() {
    this.companyDashboardService.getStockByTicker(this.ticker()).subscribe({
      next: response => {
        this.stock.set(response);
        this.getRelatedCompanies();
        this.getIncomeStatement();
      },
      error: err => {
        console.log(err);
        this.router.navigateByUrl('/companies');
      }
    });
  }

  getRelatedCompanies() {
    this.companyDashboardService.getRelatedCompanies(this.ticker()).subscribe({
      next: response => this.relatedCompanies.set(response),
      error: err => console.log(err)
    });
  }

  getIncomeStatement() {
    this.companyDashboardService.getIncomeStatement(this.ticker(), this.period).subscribe({
      next: response => this.incomeStatement.set(response),
      error: err => console.log(err)
    })
  }
}
