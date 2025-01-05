import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Pagination } from '../../shared/models/pagination';
import { Post } from '../../shared/models/post';


// services are initialized when application starts (singleton)
@Injectable({
  providedIn: 'root'
})
export class FeedService {
  private baseUrl = 'https://localhost:6001/api/';
  private http = inject(HttpClient);

  // tickers, sentiments, etc. are returned as static lists from the api server, which means
  // we need to just call them once, service is a good place to call instead of a component
  tickers: string[] = [];
  sentiments: string[] = [];

  public getPosts() {
    return this.http.get<Pagination<Post>>(this.baseUrl + 'posts?pageSize=52');
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
