import { Component, signal, WritableSignal } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzButtonComponent } from 'ng-zorro-antd/button';
import { NzInputDirective } from 'ng-zorro-antd/input';

@Component({
  selector: 'app-chart-engine',
  imports: [
    FormsModule,
    NzButtonComponent,
    NzInputDirective,
    ReactiveFormsModule
  ],
  templateUrl: './chart-engine.component.html',
  styleUrl: './chart-engine.component.scss'
})
export class ChartEngineComponent {
  ticker: WritableSignal<string | undefined> = signal(undefined);

  availableMetrics: string[] = ['revenue', 'netIncome', 'grossProfit',
    'totalAssets', 'totalLiabilities', 'totalStockholdersEquity',
    'freeCashFlow', 'stockBasedCompensation', 'cashAtEndOfPeriod'];

  onTickerSubmit() {
    this.ticker.set(this.ticker()?.toUpperCase());
    console.log(this.ticker());
  }
}
