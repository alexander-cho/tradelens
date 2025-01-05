import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './layout/navbar/navbar.component';
import { FeedComponent } from './features/feed/feed.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NavbarComponent, FeedComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'TradeLens';
}
