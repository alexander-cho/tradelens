import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { CongressTrades } from '../../shared/models/fmp';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CongressService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  public getHouseTrades(): Observable<CongressTrades[]> {
    return this.http.get<CongressTrades[]>(this.baseUrl + 'congress/trades?chamber=house');
  }

  public getSenateTrades(): Observable<CongressTrades[]> {
    return this.http.get<CongressTrades[]>(this.baseUrl + 'congress/trades?chamber=senate');
  }
}
