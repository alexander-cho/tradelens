import { Component, inject, input, InputSignal, OnInit, signal, WritableSignal } from '@angular/core';
import { OptionsChain } from '../../../shared/models/options/options-chain';
import { OptionsService } from '../../../core/services/options.service';


@Component({
  selector: 'app-options-chain-detail',
  imports: [],
  templateUrl: './options-chain-detail.component.html',
  styleUrl: './options-chain-detail.component.scss'
})
export class OptionsChainDetailComponent implements OnInit {
  ticker: InputSignal<string> = input.required<string>();
  expiration: InputSignal<string> = input.required<string>();

  optionsService = inject(OptionsService);

  optionsChainData: WritableSignal<OptionsChain | undefined> = signal<OptionsChain | undefined>(undefined);

  ngOnInit() {
    this.getOptionsChain();
  }

  getOptionsChain() {
    this.optionsService.getOptionsChain(this.ticker(), this.expiration(), true).subscribe({
      next: response => {
        this.optionsChainData.set(response);
        console.log(response);
      },
      error: err => console.log(err)
    });
  }
}
