import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EarningsCalendarModel } from '../../shared/models/earnings-calendar-model';

@Injectable({
  providedIn: 'root'
})
export class EarningsCalendarService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  public getCongressionalTrades(from: string, to: string): Observable<EarningsCalendarModel[]> {
    let params = new HttpParams();
    params = params.append('from', from).append('to', to);
    return this.http.get<EarningsCalendarModel[]>(this.baseUrl + 'market-data/earnings-calendar', { params })
  }
}
