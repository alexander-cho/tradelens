import { Component, effect, inject, input, OnInit, signal, WritableSignal } from '@angular/core';
import { Stock } from "../../shared/models/stock";
import { RelatedCompanies } from '../../shared/models/polygon';
import { CompanyDashboardService } from '../../core/services/company-dashboard.service';
import { Router } from '@angular/router';
import { CompanyFundamentalsResponse } from '../../shared/models/fundamentals/company-fundamentals-response';
import { CompanyMetricChartComponent } from '../../shared/components/company-metric-chart/company-metric-chart.component';
import { NzButtonComponent } from 'ng-zorro-antd/button';
// import { CandlestickChartComponent } from './candlestick-chart/candlestick-chart.component';

@Component({
  selector: 'app-company-dashboard',
  imports: [
    CompanyMetricChartComponent,
    NzButtonComponent,
    // CompanyMetricChartComponent,
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
  fundamentalData: WritableSignal<CompanyFundamentalsResponse | undefined> = signal(undefined);
  stock: WritableSignal<Stock | undefined> = signal(undefined);

  period: WritableSignal<string> = signal('quarter');
  metricsList: WritableSignal<string[]> = signal(['revenue', 'netIncome', 'freeCashFlow']);

  ngOnInit() {
    this.companyDashboardService.getStockByTicker(this.ticker()).subscribe({
      next: response => {
        this.stock.set(response);
        this.getRelatedCompanies();
      },
      error: err => {
        console.log(err);
        this.router.navigateByUrl('/companies');
      }
    });
  }

  // change metrics reactively
  metricsChangeEffect = effect(() => {
    const changedPeriod = this.period();
    const changedMetricsList = this.metricsList();

    if (changedPeriod || changedMetricsList) {
      this.getUserRequestedCompanyFundamentalData();
    }
  });

  onMetricsChange() {
    this.metricsList.set(['revenue', 'netIncome', 'freeCashFlow', 'grossProfit']);
  }

  getRelatedCompanies() {
    this.companyDashboardService.getRelatedCompanies(this.ticker()).subscribe({
      next: response => this.relatedCompanies.set(response),
      error: err => console.log(err)
    });
  }

  getUserRequestedCompanyFundamentalData() {
    this.companyDashboardService.getCompanyFundamentalData(this.ticker(), this.period(), this.metricsList()).subscribe({
      next: response => this.fundamentalData.set(response),
      error: err => console.log(err)
    });
  }
}
