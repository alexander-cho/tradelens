import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BarAggregates, RelatedCompanies } from '../../shared/models/polygon';
import { Stock } from '../../shared/models/stock';

@Injectable({
  providedIn: 'root'
})
export class CompanyDashboardService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  // think of creating this in a different service, one dedicated to getting bar aggs possibly
  public getBarAggregates(ticker: string, multiplier: number, timespan: string, from: string, to: string): Observable<BarAggregates> {
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
    return this.http.get<BarAggregates>(this.baseUrl + 'baraggregates', { params });
  }

  public getRelatedCompanies(ticker: string): Observable<RelatedCompanies> {
    let params = new HttpParams();
    if (ticker) {
      params = params.append('ticker', ticker);
    }
    return this.http.get<RelatedCompanies>(this.baseUrl + 'companies/related-companies', { params });
  }

  // method in order to associate ticker retrieved from parent component with stock entity
  public getStockByTicker(ticker: string): Observable<Stock> {
    return this.http.get<Stock>(this.baseUrl + 'stocks/' + ticker);
  }
}
