import { Component, inject, input, InputSignal, OnInit, signal, WritableSignal } from '@angular/core';
import { StockPriceChartSnapshotComponent } from "../stock-price-chart-snapshot/stock-price-chart-snapshot.component";
import { CompanyDashboardService } from '../../../core/services/company-dashboard.service';
import { Stock } from '../../../shared/models/stock';
import { Router } from '@angular/router';
import { NzCardComponent } from 'ng-zorro-antd/card';
import { CompanyProfile, FinancialRatios, KeyMetrics } from '../../../shared/models/fundamentals/company-profile';
import { DecimalPipe, NgOptimizedImage } from '@angular/common';
import { NzButtonComponent } from 'ng-zorro-antd/button';
import { NzIconDirective } from 'ng-zorro-antd/icon';

@Component({
  selector: 'app-company-profile',
  imports: [
    StockPriceChartSnapshotComponent,
    NzCardComponent,
    DecimalPipe,
    NgOptimizedImage,
    NzButtonComponent,
    NzIconDirective
  ],
  templateUrl: './company-profile.component.html',
  styleUrl: './company-profile.component.scss'
})
export class CompanyProfileComponent implements OnInit {
  public ticker: InputSignal<string> = input.required<string>();

  protected stock: WritableSignal<Stock | undefined> = signal(undefined);
  protected relatedCompanies: WritableSignal<string[] | undefined> = signal(undefined);
  protected companyProfile: WritableSignal<CompanyProfile | undefined> = signal(undefined);
  protected keyMetrics: WritableSignal<KeyMetrics | undefined> = signal(undefined);
  protected financialRatios: WritableSignal<FinancialRatios | undefined> = signal(undefined);

  private companyDashboardService = inject(CompanyDashboardService);
  private router = inject(Router);

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

    this.getCompanyProfile();
    this.getKeyMetrics();
    this.getFinancialRatios();
  }

  private getRelatedCompanies() {
    this.companyDashboardService.getRelatedCompanies(this.ticker()).subscribe({
      next: response => {
        this.relatedCompanies.set(response);
        console.log("Related companies: ", this.relatedCompanies());
      },
      error: err => console.log(err)
    });
  }

  private getCompanyProfile() {
    this.companyDashboardService.getCompanyProfile(this.ticker()).subscribe({
      next: response => this.companyProfile.set(response),
      error: err => console.log(err)
    });
  }

  private getKeyMetrics() {
    this.companyDashboardService.getKeyMetrics(this.ticker()).subscribe({
      next: response => this.keyMetrics.set(response),
      error: err => console.log(err)
    });
  }

  private getFinancialRatios() {
    this.companyDashboardService.getFinancialRatios(this.ticker()).subscribe({
      next: response => this.financialRatios.set(response),
      error: err => console.log(err)
    });
  }
}
