import { Component, inject, model, ModelSignal, WritableSignal } from '@angular/core';
import { NzButtonComponent } from "ng-zorro-antd/button";
import { NZ_MODAL_DATA, NzModalRef } from 'ng-zorro-antd/modal';
import { FormsModule } from '@angular/forms';
import { NzCheckboxComponent, NzCheckboxGroupComponent } from 'ng-zorro-antd/checkbox';
import { NzColDirective, NzRowDirective } from 'ng-zorro-antd/grid';

@Component({
  selector: 'app-select-metrics-modal',
  imports: [
    NzButtonComponent,
    FormsModule,
    NzCheckboxGroupComponent,
    NzRowDirective,
    NzColDirective,
    NzCheckboxComponent
  ],
  templateUrl: './select-metrics-modal.component.html',
  styleUrl: './select-metrics-modal.component.scss'
})
export class SelectMetricsModalComponent {
  private modalRef = inject(NzModalRef<SelectMetricsModalComponent>);

  // inject data passed through nzData in modalService.create() call in parent component
  data = inject(NZ_MODAL_DATA);

  metricsList: WritableSignal<string[]> = this.data.availableMetrics;
  userSelectedMetrics: ModelSignal<string[]> = model<string[]>(this.data.selectedMetrics);

  protected applyMetricsSelect() {
    this.modalRef.close({
      selectedMetrics: this.userSelectedMetrics()
    });
    console.log(this.userSelectedMetrics());
  }

  // separate utils!
  protected transformMetricName = (originalMetric: string) => {
    if(originalMetric != 'EPS') {
      const splitMetricName = originalMetric.split('');
      let newMetricName = '';
      if (splitMetricName != null) {
        for (let i = 0; i < splitMetricName.length; i++) {
          // check for uppercase letters, except for the first one
          if (i !== 0 && splitMetricName[i] === splitMetricName[i].toUpperCase() && splitMetricName[i] !== splitMetricName[i].toLowerCase()) {
            newMetricName = newMetricName + ' ' + splitMetricName[i];
          } else {
            newMetricName = newMetricName + '' + splitMetricName[i];
          }
        }
      }
      return newMetricName;
    } else {
      return 'EPS';
    }
  };

  protected clearMetricSelections() {
    this.userSelectedMetrics.set([]);
  }

  protected selectAllMetrics() {
    this.userSelectedMetrics.set(this.metricsList());
  }
}
