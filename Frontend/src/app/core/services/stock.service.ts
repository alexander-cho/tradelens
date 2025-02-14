import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Stock } from '../../shared/models/stock';

@Injectable({
  providedIn: 'root'
})
export class StockService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  getStocks() {
    return this.http.get<Stock[]>(this.baseUrl + 'stocks');
  }
}
