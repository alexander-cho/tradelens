import { Component, inject, OnInit } from '@angular/core';
import { OptionsService } from '../../core/services/options.service';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { ExpirationDates } from '../../shared/models/options';

@Component({
  selector: 'app-options',
  imports: [
    NavbarComponent
  ],
  templateUrl: './options.component.html',
  styleUrl: './options.component.scss'
})
export class OptionsComponent implements OnInit {
  optionsService = inject(OptionsService);
  ticker = 'SPY';
  expirationDates?: ExpirationDates;

  ngOnInit() {
    this.getExpirationDatesAssociatedWithStock();
  }

  getExpirationDatesAssociatedWithStock() {
    this.optionsService.getExpirations(this.ticker).subscribe({
      next: response => {
        this.expirationDates = response;
        console.log(response);
      },
      error: err => console.log(err)
    });
  }
}
