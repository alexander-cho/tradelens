import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { MarketStatus } from '../../shared/models/finnhub';

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  public getMarketStatus() {
    return this.http.get<MarketStatus>(this.baseUrl + 'marketdata/market-status');
  }
}
