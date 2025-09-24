import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ExpiryData } from '../../shared/models/options';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OptionsService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  public getExpirations(ticker: string): Observable<ExpiryData> {
    let params = new HttpParams();
    params = params.append('symbol', ticker);

    return this.http.get<ExpiryData>(this.baseUrl + 'options/expirations', { params });
  }

  public getMaxPain(ticker: string, expiration: string, greeks: boolean) {

  }
}
