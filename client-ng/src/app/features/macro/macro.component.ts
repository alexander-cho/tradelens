import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { MacroService } from '../../core/services/macro.service';
import { MarginBalance } from '../../shared/models/macro';
import { NzCardComponent } from 'ng-zorro-antd/card';

@Component({
  selector: 'app-macro',
  imports: [
    NzCardComponent
  ],
  templateUrl: './macro.component.html',
  styleUrl: './macro.component.scss'
})
export class MacroComponent implements OnInit {
  macroService = inject(MacroService);

  marginBalance: WritableSignal<MarginBalance | undefined> = signal(undefined);
  moneyMarketFunds: WritableSignal<MarginBalance | undefined> = signal(undefined);

  ngOnInit() {
    this.getMarginBalance();
    this.getMoneyMarketFunds();
  }

  getMarginBalance() {
    this.macroService.getMarginBalance().subscribe({
      next: response => this.marginBalance.set(response),
      error: err => console.log(err)
    });
  }

  getMoneyMarketFunds() {
    this.macroService.getMoneyMarketFunds().subscribe({
      next: response => this.moneyMarketFunds.set(response),
      error: err => console.log(err)
    })
  }
}
