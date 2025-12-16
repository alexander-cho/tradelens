import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MarketStatus } from '../../shared/models/finnhub';
import { FinnhubCompanyProfile } from '../../shared/models/fundamentals/finnhub-company-profile';

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  public getMarketStatus() {
    return this.http.get<MarketStatus>(this.baseUrl + 'market-data/market-status');
  }

  public getFinnhubCompanyProfile(ticker: string) {
    let params = new HttpParams();
    if (ticker) {
      params = params.append('ticker', ticker);
    }

    return this.http.get<FinnhubCompanyProfile>(this.baseUrl + 'companies/company-profile-finnhub', { params })
  }
}
