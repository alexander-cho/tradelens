import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { HomeService } from '../../core/services/home.service';
import { MarketStatus } from '../../shared/models/finnhub';
import { CompanyDashboardService } from '../../core/services/company-dashboard.service';
import { NzCardComponent } from 'ng-zorro-antd/card';
import { FinnhubCompanyProfile } from '../../shared/models/fundamentals/finnhub-company-profile';
import { RouterLink } from '@angular/router';
import { DatePipe, DecimalPipe, NgOptimizedImage } from '@angular/common';
import { NzColDirective, NzRowDirective } from 'ng-zorro-antd/grid';
import { forkJoin, switchMap } from 'rxjs';

@Component({
  selector: 'app-home',
  imports: [
    NzCardComponent,
    RouterLink,
    NgOptimizedImage,
    NzColDirective,
    NzRowDirective,
    DecimalPipe,
    DatePipe
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  private homeService = inject(HomeService);
  private companyDashboardService = inject(CompanyDashboardService);

  protected marketStatus: WritableSignal<MarketStatus | undefined> = signal(undefined);
  protected availableCompanyProfiles: WritableSignal<FinnhubCompanyProfile[] | undefined> = signal(undefined);

  ngOnInit() {
    this.getMarketStatus();
    this.getAvailableCompaniesProfiles();
  }

  private getMarketStatus() {
    this.homeService.getMarketStatus().subscribe({
      next: response => this.marketStatus.set(response),
      error: err => console.log(err)
    });
  }

  // the source observable, the list of available companies will be used to create the array of new observables
  // switchMap receives as its parameter the result emitted by the getAvailableCompanies() observable, string[]
  // use that array to create new observables getCompanyProfile(company) and switchMap "switches" to this new
  // observable via the forkJoin(), then subscribes to it

  private getAvailableCompaniesProfiles() {
    this.companyDashboardService.getAvailableCompanies().pipe(
      switchMap(availableCompanies => {
        return forkJoin(
          availableCompanies.map(company => {
            return this.homeService.getFinnhubCompanyProfile(company);
          })
        )
      })
    ).subscribe({
      next: response => this.availableCompanyProfiles.set(response),
      error: err => console.log(err)
    });
  }
}
