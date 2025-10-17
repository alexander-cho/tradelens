import { Component, inject, signal, WritableSignal } from '@angular/core';
import { OptionsService } from '../../core/services/options.service';
import { FormsModule } from '@angular/forms';
import { CallsAndPutsCashSums, ExpiryData } from '../../shared/models/options';
import { MaxPainChartComponent } from './max-pain-chart/max-pain-chart.component';
import { NzButtonComponent } from 'ng-zorro-antd/button';
import { NzInputDirective } from 'ng-zorro-antd/input';
import { NzOptionComponent, NzSelectComponent } from 'ng-zorro-antd/select';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-options',
  imports: [
    FormsModule,
    MaxPainChartComponent,
    NzButtonComponent,
    NzInputDirective,
    NzSelectComponent,
    NzOptionComponent,
    RouterLink
  ],
  templateUrl: './options.component.html',
  styleUrl: './options.component.scss'
})
export class OptionsComponent {
  optionsService = inject(OptionsService);

  ticker: WritableSignal<string> = signal<string>("");
  selectedExpiration: WritableSignal<string | undefined> = signal<string | undefined>(undefined);
  expirationDates: WritableSignal<ExpiryData | undefined> = signal<ExpiryData | undefined>(undefined);
  maxPainData: WritableSignal<CallsAndPutsCashSums | undefined> = signal<CallsAndPutsCashSums | undefined>(undefined);

  // only pass ticker to child component when user submits it,
  // or else signal is updated for each letter change inside of input
  submittedTicker: WritableSignal<string> = signal<string>("");

  onTickerSubmit() {
    this.submittedTicker.set(this.ticker());
    this.getExpirationDatesAssociatedWithStock();
  }

  onExpiryDateSelection() {
    this.getCashValuesAndMaxPain();
  }

  getExpirationDatesAssociatedWithStock() {
    // reset the selected expiration when getting new ticker data
    this.selectedExpiration.set("");

    // reset maxPainData (cash value data) when user selects a new ticker
    // or else chart re-renders with stale data
    this.maxPainData.set(undefined);

    this.optionsService.getExpirations(this.ticker()).subscribe({
      next: response => {
        this.expirationDates.set(response);
        console.log(this.ticker());
        console.log(response);
      },
      error: err => console.log(err)
    });
  }

  getCashValuesAndMaxPain() {
    this.optionsService.getCashValuesAndMaxPain(this.ticker(), this.selectedExpiration()).subscribe({
      next: response => {
        this.maxPainData.set(response);
        console.log(this.selectedExpiration());
        console.log(response);
      },
      error: err => console.log(err)
    });
  }
}
