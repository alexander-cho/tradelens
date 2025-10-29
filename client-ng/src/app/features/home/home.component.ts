import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { HomeService } from '../../core/services/home.service';
import { MarketStatus } from '../../shared/models/finnhub';

@Component({
  selector: 'app-home',
  imports: [
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  homeService = inject(HomeService);

  marketStatus: WritableSignal<MarketStatus | undefined> = signal(undefined);

  ngOnInit() {
    this.getMarketStatus();
  }

  getMarketStatus() {
    this.homeService.getMarketStatus().subscribe({
      next: response => this.marketStatus.set(response),
      error: err => console.log(err)
    });
  }
}
