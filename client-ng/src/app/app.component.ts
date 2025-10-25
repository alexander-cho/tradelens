import { Component, inject, signal } from '@angular/core';
import { NavigationEnd, Router, RouterLink, RouterOutlet } from '@angular/router';
import { filter } from 'rxjs';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzMenuDirective, NzMenuItemComponent,
  // NzSubMenuComponent
} from 'ng-zorro-antd/menu';
import {
  NzContentComponent,
  NzFooterComponent,
  NzHeaderComponent,
  NzLayoutModule,
  NzSiderComponent
} from 'ng-zorro-antd/layout';
import { NzCardComponent } from 'ng-zorro-antd/card';
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    NzIconModule,
    NzMenuDirective,
    NzMenuItemComponent,
    NzSiderComponent,
    // NzSubMenuComponent,
    NzLayoutModule,
    NzHeaderComponent,
    NzContentComponent,
    NzFooterComponent,
    RouterLink,
    NzCardComponent,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = signal("TradeLens");
  protected authService = inject(AuthService);
  isCollapsed = false;

  showNavbar = true;
  private hiddenRoutes = ['/login', '/auth/register', '/auth/landing'];

  constructor(private router: Router) {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        this.showNavbar = !this.hiddenRoutes.includes(event.url);
      });
  }

  logout() {
    this.authService.logout().subscribe({
      next: () => {
        this.authService.currentUser.set(null);
        this.router.navigateByUrl('/');
      }
    })
  }
}
