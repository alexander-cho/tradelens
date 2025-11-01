import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
// import { NzIconDirective } from 'ng-zorro-antd/icon';

@Component({
  selector: 'app-navbar',
  imports: [ RouterLink,
    // NzIconDirective
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  protected authService = inject(AuthService);
  private router = inject(Router);

  logout() {
    this.authService.logout().subscribe({
      next: () => {
        this.authService.currentUser.set(null);
        this.router.navigateByUrl('/');
      }
    })
  }
}
