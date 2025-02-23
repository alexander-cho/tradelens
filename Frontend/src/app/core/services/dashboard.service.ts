import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BarAggregates, RelatedCompanies } from '../../shared/models/polygon';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  public getBarAggregates(ticker: string, multiplier: number, timespan: string, from: string, to: string) {
    let params = new HttpParams();
    if (ticker) {
      params = params.append('ticker', ticker);
    }
    if (multiplier) {
      params = params.append('multiplier', multiplier);
    }
    if (timespan) {
      params = params.append('timespan', timespan);
    }
    if (from) {
      params = params.append('from', from);
    }
    if (to) {
      params = params.append('to', to);
    }
    return this.http.get<BarAggregates>(this.baseUrl + 'polygon/baraggs', { params });
  }

  public getRelatedCompanies(ticker: string) {
    let params = new HttpParams();
    if (ticker) {
      params = params.append('ticker', ticker);
    }
    return this.http.get<RelatedCompanies>(this.baseUrl + 'polygon/relatedcompanies', { params });
  }
}
