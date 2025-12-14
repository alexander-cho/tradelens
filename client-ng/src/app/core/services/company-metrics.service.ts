import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CompanyMetricDto } from '../../shared/models/fundamentals/company-metric-dto';

@Injectable({
  providedIn: 'root'
})
export class CompanyMetricsService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  public getAllMetrics(ticker: string | undefined, interval: string, metric: string, from?: string, to?: string): Observable<CompanyMetricDto> {
    let params = new HttpParams();
    if (ticker) {
      params = params
        .append('ticker', ticker)
        .append('interval', interval);
    }
    if (metric) {
      params = params.append('metric', metric);
    }

    if (from) {
      params = params.append('from', from);
    }

    if (to) {
      params = params.append('to', to);
    }

    return this.http.get<CompanyMetricDto>(this.baseUrl + 'company-metrics/all-metrics', { params });
  }

  public getAllMetricNamesAssociatedWithTicker(ticker: string): Observable<string[]> {
    let params = new HttpParams();
    if (ticker) {
      params = params.append('ticker', ticker);
    }

    return this.http.get<string[]>(this.baseUrl + 'company-metrics/available-metrics', { params })
  }
}
