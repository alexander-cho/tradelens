import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Pagination } from '../../shared/models/pagination';
import { Post } from '../../shared/models/post';
import { environment } from '../../../environments/environment';


// services are initialized when application starts (singleton)
@Injectable({
  providedIn: 'root'
})
export class FeedService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  // tickers, sentiments, etc. are returned as static lists from the api server, which means
  // we need to just call them once, service is a good place to call instead of a component
  tickers: string[] = [];
  sentiments: string[] = [];

  public getPosts(tickers?: string[], sentiments?: string[]) {
    // build query string as parameter object
    let params = new HttpParams();
    if (tickers && tickers.length > 0) {
      params = params.append('tickers', tickers.join(','));
    }
    if (sentiments && sentiments.length > 0) {
      params = params.append('sentiments', sentiments.join(','));
    }
    params = params.append('pageSize', 57);
    return this.http.get<Pagination<Post>>(this.baseUrl + 'posts', { params });
  }

  public getTickers() {
    // if (this.tickers.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + 'posts/tickers').subscribe({
      next: response => this.tickers = response
    });
  }

  public getSentiments() {
    // if (this.sentiments.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + 'posts/sentiments').subscribe({
      next: response => this.sentiments = response
    })
  }
}
