import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MarginBalance, MoneyMarketFunds } from '../../shared/models/macro';

@Injectable({
  providedIn: 'root'
})
export class MacroService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  public getMarginBalance(): Observable<MarginBalance> {
    return this.http.get<MarginBalance>(this.baseUrl + 'macro/margin-balance');
  }

  public getMoneyMarketFunds(): Observable<MoneyMarketFunds> {
    return this.http.get<MoneyMarketFunds>(this.baseUrl + 'macro/money-market');
  }
}
