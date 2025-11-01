import { Component, inject, model, ModelSignal, signal, WritableSignal } from '@angular/core';
// import { NzButtonComponent } from 'ng-zorro-antd/button';
// import { NzOptionComponent, NzSelectComponent } from 'ng-zorro-antd/select';
import { FormsModule } from '@angular/forms';
import { NZ_MODAL_DATA, NzModalRef } from 'ng-zorro-antd/modal';
import { NzButtonComponent } from 'ng-zorro-antd/button';
import { NzInputDirective } from 'ng-zorro-antd/input';

@Component({
  selector: 'app-ad-hoc-chart-modal',
  imports: [
    // NzButtonComponent,
    // NzOptionComponent,
    // NzSelectComponent,
    FormsModule,
    NzButtonComponent,
    NzInputDirective
  ],
  templateUrl: './ad-hoc-chart-modal.component.html',
  styleUrl: './ad-hoc-chart-modal.component.scss'
})
export class AdHocChartModalComponent {
  modalRef = inject(NzModalRef<AdHocChartModalComponent>);

  data = inject(NZ_MODAL_DATA);

  // string needs parsing by comma separation
  // convert incoming arrays to comma-separated strings for the input fields
  rawXAxisInput: WritableSignal<string | undefined> = signal(this.data.chartXValues?.join(', ') ?? '');
  rawYDataInput: WritableSignal<string | undefined> = signal(this.data.chartYValues?.join(', ') ?? '');

  userEnteredChartTitle: ModelSignal<string> = model(this.data.chartTitle);
  userEnteredYLabel: ModelSignal<string> = model(this.data.chartYLabel);

  applyChartOptions() {
    // iterate over each element of array and parse
    let userEnteredXAxisDataLabels: string[] | undefined = this.rawXAxisInput()?.split(',').map(s => s.trim());
    let userEnteredYData: number[] | undefined = this.rawYDataInput()?.split(',').map(s => parseFloat(s.trim()));

    this.modalRef.close({
      chartTitle: this.userEnteredChartTitle(),
      chartXAxis: userEnteredXAxisDataLabels,
      chartYVals: userEnteredYData,
      chartYLabel: this.userEnteredYLabel()
    });
    console.log(this.userEnteredChartTitle());
    console.log(this.data.chartXValues);
  }
}
