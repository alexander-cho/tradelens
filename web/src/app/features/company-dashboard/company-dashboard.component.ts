import { Component, inject, input, OnInit } from '@angular/core';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { DashboardService } from '../../core/services/dashboard.service';
import { RelatedCompanies } from '../../shared/models/polygon';
import { CandlestickChartComponent } from './candlestick-chart/candlestick-chart.component';
import { RouterLink } from '@angular/router';

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

  relatedCompanies?: RelatedCompanies;

  ngOnInit() {
    this.getRelatedCompanies();
  }

  getRelatedCompanies() {
    this.dashboardService.getRelatedCompanies(this.ticker()).subscribe({
      next: response => {
        this.relatedCompanies = response;
        console.log(response);
      },
      error: err => console.log(err)
    })
  }
}
