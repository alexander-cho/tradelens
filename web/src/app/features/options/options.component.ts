import { Component, inject } from '@angular/core';
import { OptionsService } from '../../core/services/options.service';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { FormsModule } from '@angular/forms';
import { CallsAndPutsCashSums, ExpiryData } from '../../shared/models/options';
import { MaxPainChartComponent } from './max-pain-chart/max-pain-chart.component';
import { NzButtonComponent } from 'ng-zorro-antd/button';
import { NzInputDirective } from 'ng-zorro-antd/input';
import { NzOptionComponent, NzSelectComponent } from 'ng-zorro-antd/select';

@Component({
  selector: 'app-options',
  imports: [
    NavbarComponent,
    FormsModule,
    MaxPainChartComponent,
    NzButtonComponent,
    NzInputDirective,
    NzSelectComponent,
    NzOptionComponent
  ],
  templateUrl: './options.component.html',
  styleUrl: './options.component.scss'
})
export class OptionsComponent {
  optionsService = inject(OptionsService);
  ticker = "";
  expirationDates?: ExpiryData;
  selectedExpiration: string = "";
  maxPainData?: CallsAndPutsCashSums;

  onTickerSubmit() {
    this.getExpirationDatesAssociatedWithStock();
  }

  onExpiryDateSelection() {
    this.getCashValuesAndMaxPain();
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

  getCashValuesAndMaxPain() {
    this.optionsService.getCashValuesAndMaxPain(this.ticker, this.selectedExpiration).subscribe({
      next: response => {
        this.maxPainData = response;
        console.log(this.selectedExpiration);
        console.log(response);
      },
      error: err => console.log(err)
    });
  }
}
