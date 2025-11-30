import { Component, inject, input, InputSignal, OnInit, signal, WritableSignal } from '@angular/core';
import { OptionsChain, StrikePriceData } from '../../../shared/models/options/options-chain';
import { OptionsService } from '../../../core/services/options.service';
import { NzTableComponent } from 'ng-zorro-antd/table';
import { NzRadioComponent, NzRadioGroupComponent } from 'ng-zorro-antd/radio';
import { FormsModule } from '@angular/forms';
import { OptionChainRow, OptionMetricData } from '../../../shared/models/options/options-data-shapes';
import {
  OptionsMetricChartComponent
} from '../../../shared/components/options-metric-chart/options-metric-chart.component';

@Component({
  selector: 'app-options-chain-detail',
  imports: [
    NzTableComponent,
    NzRadioComponent,
    NzRadioGroupComponent,
    FormsModule,
    OptionsMetricChartComponent,
  ],
  templateUrl: './options-chain-detail.component.html',
  styleUrl: './options-chain-detail.component.scss'
})
export class OptionsChainDetailComponent implements OnInit {
  ticker: InputSignal<string> = input.required<string>();
  expiration: InputSignal<string> = input.required<string>();

  // show options chain table initially
  selectedMetric: WritableSignal<string> = signal("optionsChain");

  optionsService = inject(OptionsService);

  optionsChainData: WritableSignal<OptionsChain | undefined> = signal<OptionsChain | undefined>(undefined);
  formattedData: WritableSignal<OptionChainRow[]> = signal<OptionChainRow[]>([]);

  optionsMetricData: WritableSignal<OptionMetricData[] | undefined> = signal<OptionMetricData[] | undefined>(undefined);

  ngOnInit() {
    this.getOptionsChain();
  }

  onMetricChangeEvent(event: keyof StrikePriceData) {
    this.selectedMetric.set(event);
    if (this.selectedMetric() != 'optionsChain') {
      this.optionsMetricData.set(this.getMetricData(event));
    } else {
      this.optionsMetricData.set(undefined);
    }
  }

  getOptionsChain() {
    this.optionsService.getOptionsChain(this.ticker(), this.expiration(), true).subscribe({
      next: response => {
        this.optionsChainData.set(response);
        this.formatOptionsData(response);
      },
      error: err => console.log(err)
    });
  }

  getMetricData(metric: keyof StrikePriceData) {
    let data: StrikePriceData[] | undefined = this.optionsChainData()?.optionsChain;
    const optionMetricData: OptionMetricData[] = [];

    if (data != null) {
      for (const metricData of data) {
        let optionMetricDataAtStrike: OptionMetricData = {
          metricType: metric,
          strike: metricData.strike,
          optionType: metricData.optionType,
          data: metricData[metric]?.valueOf()
        };
        optionMetricData.push(optionMetricDataAtStrike);
      }
    }
    return optionMetricData;
  }

  formatOptionsData(data: OptionsChain) {
    const strikeMap = new Map<number, OptionChainRow>();

    data.optionsChain.forEach(option => {
      if (!strikeMap.has(option.strike)) {
        strikeMap.set(option.strike, { strike: option.strike });
      }

      const row = strikeMap.get(option.strike)!;

      if (option.optionType === 'call') {
        row.call = {
          last: option.last,
          bid: option.bid,
          ask: option.ask,
          volume: option.volume,
          openInterest: option.openInterest
        };
      } else if (option.optionType === 'put') {
        row.put = {
          last: option.last,
          bid: option.bid,
          ask: option.ask,
          volume: option.volume,
          openInterest: option.openInterest
        };
      }
    });

    this.formattedData.set(Array.from(strikeMap.values()).sort((a, b) => a.strike - b.strike));
  }
}
