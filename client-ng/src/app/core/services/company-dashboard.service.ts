import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BarAggregates } from '../../shared/models/polygon';
import { Stock } from '../../shared/models/stock';
import { CompanyFundamentalsResponse } from '../../shared/models/fundamentals/company-fundamentals-response';
import { CompanyProfile, FinancialRatios, KeyMetrics } from '../../shared/models/fundamentals/company-profile';

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

    return this.http.get<BarAggregates>(this.baseUrl + 'bar-aggregates', { params });
  }

  // method in order to associate ticker retrieved from parent component with stock entity
  public getStockByTicker(ticker: string): Observable<Stock> {
    return this.http.get<Stock>(this.baseUrl + 'stocks/' + ticker);
  }

  public getRelatedCompanies(ticker: string | undefined): Observable<string[]> {
    let params = new HttpParams();
    if (ticker) {
      params = params.append('ticker', ticker);
    }

    return this.http.get<string[]>(this.baseUrl + 'companies/related-companies', { params });
  }

  public getCompanyFundamentalData(ticker: string | undefined, period: string, metric: string[]): Observable<CompanyFundamentalsResponse> {
    let params = new HttpParams();
    if (ticker) {
      params = params
        .append('ticker', ticker)
        .append('period', period);
    }
    if (metric) {
      for (const companyMetric of metric) {
        params = params.append('metric', companyMetric);
      }
    }

    return this.http.get<CompanyFundamentalsResponse>(this.baseUrl + 'companies', { params });
  }

  public getCompanyProfile(ticker: string): Observable<CompanyProfile> {
    let params = new HttpParams();
    if (ticker) {
      params = params.append('ticker', ticker);
    }

    return this.http.get<CompanyProfile>(this.baseUrl + 'companies/company-profile', { params });
  }

  public getKeyMetrics(ticker: string): Observable<KeyMetrics> {
    let params = new HttpParams();
    if (ticker) {
      params = params.append('ticker', ticker);
    }

    return this.http.get<KeyMetrics>(this.baseUrl + 'companies/key-metrics', { params });
  }

  public getFinancialRatios(ticker: string): Observable<FinancialRatios> {
    let params = new HttpParams();
    if (ticker) {
      params = params.append('ticker', ticker);
    }

    return this.http.get<FinancialRatios>(this.baseUrl + 'companies/financial-ratios', { params });
  }

  public getCompanyMetricsGroupedByParent(ticker: string | undefined, interval: string, metric: string[], from?: string, to?: string): Observable<CompanyFundamentalsResponse> {
    let params = new HttpParams();
    if (ticker) {
      params = params
        .append('ticker', ticker)
        .append('interval', interval);
    }
    if (metric) {
      for (const companyMetric of metric) {
        params = params.append('metric', companyMetric);
      }
    }

    if (from) {
      params = params.append('from', from);
    }

    if (to) {
      params = params.append('to', to);
    }

    return this.http.get<CompanyFundamentalsResponse>(this.baseUrl + 'company-metrics/grouped-parent', { params });
  }

  public getParentMetricsAssociatedWithTicker(ticker: string): Observable<string[]> {
    let params = new HttpParams();
    if (ticker) {
      params = params.append('ticker', ticker);
    }

    return this.http.get<string[]>(this.baseUrl + 'company-metrics/metrics', { params })
  }

  public getAvailableCompanies(): Observable<string[]> {
    return this.http.get<string[]>(this.baseUrl + 'company-metrics/available-companies')
  }
}
