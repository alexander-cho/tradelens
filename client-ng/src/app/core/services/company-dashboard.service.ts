import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BarAggregates, RelatedCompanies } from '../../shared/models/polygon';
import { Stock } from '../../shared/models/stock';
import { IncomeStatement } from '../../shared/models/fundamentals/income-statement';
import { BalanceSheet } from '../../shared/models/fundamentals/balance-sheet';
import { CashFlowStatement } from '../../shared/models/fundamentals/cash-flow-statement';

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

  public getRelatedCompanies(ticker: string | undefined): Observable<RelatedCompanies> {
    let params = new HttpParams();
    if (ticker) {
      params = params.append('ticker', ticker);
    }
    return this.http.get<RelatedCompanies>(this.baseUrl + 'companies/related-companies', { params });
  }

  public getIncomeStatement(ticker: string | undefined, period: string): Observable<IncomeStatement> {
    let params = new HttpParams();
    if (ticker) {
      params = params
        .append('ticker', ticker)
        .append('period', period);
    }
    return this.http.get<IncomeStatement>(this.baseUrl + 'companies/income-statement', { params });
  }

  public getBalanceSheet(ticker: string | undefined, period: string): Observable<BalanceSheet> {
    let params = new HttpParams();
    if (ticker) {
      params = params
        .append('ticker', ticker)
        .append('period', period);
    }
    return this.http.get<BalanceSheet>(this.baseUrl + 'companies/balance-sheet', { params });
  }

  public getCashFlowStatement(ticker: string | undefined, period: string): Observable<CashFlowStatement> {
    let params = new HttpParams();
    if (ticker) {
      params = params
        .append('ticker', ticker)
        .append('period', period);
    }
    return this.http.get<CashFlowStatement>(this.baseUrl + 'companies/cash-flow', { params });
  }
}
