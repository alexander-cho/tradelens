import { Injectable, inject, signal } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient, HttpParams } from '@angular/common/http';
import { User } from '../../shared/models/user';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  currentUser = signal<User | null>(null);

  login(values: any) {
    let params = new HttpParams();
    params = params.append('useCookies', true);
    console.log(this.baseUrl);
    return this.http.post<User>(this.baseUrl + 'login', values, { params });
  }

  register(values: any) {
    return this.http.post(this.baseUrl + 'auth/register', values);
  }

  getUserInfo() {
    return this.http.get<User>(this.baseUrl + 'auth/user-info').pipe(
      map(user => {
        this.currentUser.set(user);
        return user;
      })
    )
  }

  logout() {
    return this.http.post(this.baseUrl + 'auth/logout', {});
  }
}
