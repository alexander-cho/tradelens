import { Component, inject, OnInit } from '@angular/core';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { HomeService } from '../../core/services/home.service';
import { MarketStatus } from '../../shared/models/finnhub';

@Component({
  selector: 'app-home',
  imports: [
    NavbarComponent
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  homeService = inject(HomeService);
  marketStatus?: MarketStatus;

  ngOnInit() {
    this.getMarketStatus();
  }

  getMarketStatus() {
    this.homeService.getMarketStatus().subscribe({
      next: response => {
        this.marketStatus = response;
      },
      error: err => console.log(err)
    });
  }
}
