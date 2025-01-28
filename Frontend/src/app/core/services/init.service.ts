import { inject, Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { forkJoin } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InitService {
  // persist logged in user
  private authService = inject(AuthService);

  init() {
    // wait for multiple observables to complete, emit their latest values as an array
    return forkJoin({
      user: this.authService.getUserInfo()
    });
  }
}
