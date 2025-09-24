import { Component, inject, OnInit } from '@angular/core';
import { OptionsService } from '../../core/services/options.service';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { ExpiryData } from '../../shared/models/options';
import { MatButton } from '@angular/material/button';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-options',
  imports: [
    NavbarComponent,
    MatButton,
    FormsModule
  ],
  templateUrl: './options.component.html',
  styleUrl: './options.component.scss'
})
export class OptionsComponent implements OnInit {
  optionsService = inject(OptionsService);
  ticker = "";
  expirationDates?: ExpiryData;

  ngOnInit() {
    // this.getExpirationDatesAssociatedWithStock();
  }

  onTickerSubmit() {
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
