import { Component, inject, OnInit } from '@angular/core';
import { OptionsService } from '../../core/services/options.service';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { MatButton } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { CallsAndPutsCashSums, ExpiryData } from '../../shared/models/options';

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
  maxPainData?: CallsAndPutsCashSums;

  ngOnInit() {
    // this.getExpirationDatesAssociatedWithStock();
  }

  onTickerSubmit() {
    this.getExpirationDatesAssociatedWithStock();
    this.getCashValuesAndMaxPAin();
  }

  // create cash values chart

  getExpirationDatesAssociatedWithStock() {
    this.optionsService.getExpirations(this.ticker).subscribe({
      next: response => {
        this.expirationDates = response;
        console.log(response);
      },
      error: err => console.log(err)
    });
  }

  getCashValuesAndMaxPAin() {
    this.optionsService.getCashValuesAndMaxPain(this.ticker, "2025-09-26").subscribe({
      next: response => {
        this.maxPainData = response;
        console.log(response);
      },
      error: err => console.log(err)
    });
  }
}
