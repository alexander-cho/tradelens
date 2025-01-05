import { Component } from '@angular/core';
import { OverlayBadgeModule } from 'primeng/overlaybadge';

@Component({
  selector: 'app-navbar',
  imports: [OverlayBadgeModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {

}
