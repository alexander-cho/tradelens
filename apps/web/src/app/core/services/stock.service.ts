import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Stock } from '../../shared/models/stock';
import { Pagination } from '../../shared/models/pagination';
import { CompanyParams } from '../../shared/models/company-params';

@Injectable({
  providedIn: 'root'
})
export class StockService {
  private baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  ipoYears: number[] = [];
  countries: string[] = [];
  sectors: string[] = [];

  getStocks(companyParams: CompanyParams) {
    let params = new HttpParams();

    // ...&ipoYears=2015&ipoYears=2016&....
    if (companyParams.ipoYears.length > 0) {
      for (const ipoYear of companyParams.ipoYears) {
        params = params.append('ipoYears', ipoYear);
      }
    }

    if (companyParams.countries.length > 0) {
      params = params.append('countries', companyParams.countries.join(','));
    }

    if (companyParams.sectors.length > 0) {
      params = params.append('sectors', companyParams.sectors.join(','));
    }

    params = params.append('pageSize', companyParams.pageSize);
    params = params.append('pageIndex', companyParams.pageNumber);

    if (companyParams.sort) {
      params = params.append('sort', companyParams.sort);
    }

    if (companyParams.search) {
      params = params.append('search', companyParams.search);
    }

    return this.http.get<Pagination<Stock>>(this.baseUrl + 'stocks', { params });
  }

  public getIpoYears() {
    // if (this.tickers.length > 0) return;
    return this.http.get<number[]>(this.baseUrl + 'stocks/ipoYears').subscribe({
      next: response => this.ipoYears = response
    });
  }

  public getCountries() {
    // if (this.tickers.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + 'stocks/countries').subscribe({
      next: response => this.countries = response
    });
  }

  public getSectors() {
    // if (this.tickers.length > 0) return;
    return this.http.get<string[]>(this.baseUrl + 'stocks/sectors').subscribe({
      next: response => this.sectors = response
    });
  }
}
