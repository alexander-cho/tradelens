import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
// import { NzLayoutComponent } from 'ng-zorro-antd/layout';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    // NzLayoutComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'TradeLens';
}
