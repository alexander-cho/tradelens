import { Component } from '@angular/core';
import { NavbarComponent } from '../../layout/navbar/navbar.component';

@Component({
  selector: 'app-home',
  imports: [
    NavbarComponent
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {

}
