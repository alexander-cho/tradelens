import { Component, inject, model, ModelSignal, WritableSignal } from '@angular/core';
import { NzButtonComponent } from "ng-zorro-antd/button";
import { NzOptionComponent, NzSelectComponent } from "ng-zorro-antd/select";
import { NZ_MODAL_DATA, NzModalRef } from 'ng-zorro-antd/modal';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-select-metrics-modal',
  imports: [
    NzButtonComponent,
    NzOptionComponent,
    NzSelectComponent,
    FormsModule
  ],
  templateUrl: './select-metrics-modal.component.html',
  styleUrl: './select-metrics-modal.component.scss'
})
export class SelectMetricsModalComponent {
  modalRef = inject(NzModalRef<SelectMetricsModalComponent>);

  // inject data passed through nzData in modalService.create() call in parent component
  data = inject(NZ_MODAL_DATA);

  metricsList: WritableSignal<string[]> = this.data.availableMetrics;
  userSelectedMetrics: ModelSignal<string[]> = model<string[]>(this.data.selectedMetrics);

  applyMetricsSelect() {
    this.modalRef.close({
      selectedMetrics: this.userSelectedMetrics()
    });
    console.log(this.userSelectedMetrics());
  }
}
