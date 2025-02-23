import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { CongressTrades } from '../../shared/models/fmp';

@Injectable({
  providedIn: 'root'
})
export class CongressService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  public getHouseTrades() {
    return this.http.get<CongressTrades[]>(this.baseUrl + 'fmp/house');
  }

  public getSenateTrades() {
    return this.http.get<CongressTrades[]>(this.baseUrl + 'fmp/senate');
  }
}
