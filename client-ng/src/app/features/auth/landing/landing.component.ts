import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NzButtonComponent } from 'ng-zorro-antd/button';
import { NgOptimizedImage } from '@angular/common';

@Component({
  selector: 'app-landing',
  imports: [
    RouterLink,
    NzButtonComponent,
    NgOptimizedImage
  ],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.scss'
})
export class LandingComponent {

}
