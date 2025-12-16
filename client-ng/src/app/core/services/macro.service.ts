import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SeriesObservations } from '../../shared/models/macro';

@Injectable({
  providedIn: 'root'
})
export class MacroService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  public getSeriesObservations(seriesId: string): Observable<SeriesObservations> {
    let params = new HttpParams();
    params = params.append('seriesId', seriesId);
    return this.http.get<SeriesObservations>(this.baseUrl + 'macro/series-observations', { params })
  }
}
