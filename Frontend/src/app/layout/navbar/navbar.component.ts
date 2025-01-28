import { Component, inject } from '@angular/core';
import { OverlayBadgeModule } from 'primeng/overlaybadge';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-navbar',
  imports: [OverlayBadgeModule, RouterLink, RouterLinkActive],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  authService = inject(AuthService);
  private router = inject(Router);

  logout() {
    this.authService.logout().subscribe({
      next: () => {
        this.authService.currentUser.set(null);
        this.router.navigateByUrl('');
      }
    })
  }
}
