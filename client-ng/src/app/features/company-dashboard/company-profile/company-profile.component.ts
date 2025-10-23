import { Component, inject, input, InputSignal, OnInit, signal, WritableSignal } from '@angular/core';
import { StockPriceChartSnapshotComponent } from "../stock-price-chart-snapshot/stock-price-chart-snapshot.component";
import { CompanyDashboardService } from '../../../core/services/company-dashboard.service';
import { Stock } from '../../../shared/models/stock';
import { Router } from '@angular/router';
import { RelatedCompanies } from '../../../shared/models/polygon';
import { NzCardComponent } from 'ng-zorro-antd/card';

@Component({
  selector: 'app-company-profile',
  imports: [
    StockPriceChartSnapshotComponent,
    NzCardComponent
  ],
  templateUrl: './company-profile.component.html',
  styleUrl: './company-profile.component.scss'
})
export class CompanyProfileComponent implements OnInit {
  ticker: InputSignal<string> = input.required<string>();
  stock: WritableSignal<Stock | undefined> = signal(undefined);
  relatedCompanies: WritableSignal<RelatedCompanies | undefined> = signal(undefined);

  companyDashboardService = inject(CompanyDashboardService);
  router = inject(Router);

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

  getRelatedCompanies() {
    this.companyDashboardService.getRelatedCompanies(this.ticker()).subscribe({
      next: response => this.relatedCompanies.set(response),
      error: err => console.log(err)
    });
  }
}
