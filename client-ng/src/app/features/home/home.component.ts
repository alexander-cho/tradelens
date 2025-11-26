import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { HomeService } from '../../core/services/home.service';
import { MarketStatus } from '../../shared/models/finnhub';
import { CompanyDashboardService } from '../../core/services/company-dashboard.service';
import { NzCardComponent } from 'ng-zorro-antd/card';

@Component({
  selector: 'app-home',
  imports: [
    NzCardComponent
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  private homeService = inject(HomeService);
  private companyDashboardService = inject(CompanyDashboardService);

  protected marketStatus: WritableSignal<MarketStatus | undefined> = signal(undefined);
  protected availableCompanies: WritableSignal<string[] | undefined> = signal(undefined);

  ngOnInit() {
    this.getMarketStatus();
    this.getAvailableCompanies();
  }

  private getMarketStatus() {
    this.homeService.getMarketStatus().subscribe({
      next: response => this.marketStatus.set(response),
      error: err => console.log(err)
    });
  }

  private getAvailableCompanies() {
    this.companyDashboardService.getAvailableCompanies().subscribe({
      next: response => this.availableCompanies.set(response),
      error: err => console.log(err)
    });
  }
}
