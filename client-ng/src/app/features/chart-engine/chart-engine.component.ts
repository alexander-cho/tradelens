import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzButtonComponent } from 'ng-zorro-antd/button';
import { Chart } from 'chart.js/auto';
import { SelectMetricsModalComponent } from '../company-dashboard/select-metrics-modal/select-metrics-modal.component';
import { NzModalService } from 'ng-zorro-antd/modal';
import { AdHocChartModalComponent } from './ad-hoc-chart-modal/ad-hoc-chart-modal.component';
import { ChartEngineChartComponent } from './chart-engine-chart/chart-engine-chart.component';
// import { NzInputDirective } from 'ng-zorro-antd/input';

@Component({
  selector: 'app-chart-engine',
  imports: [
    FormsModule,
    NzButtonComponent,
    // NzInputDirective,
    ReactiveFormsModule,
    ChartEngineChartComponent
  ],
  providers: [NzModalService],
  templateUrl: './chart-engine.component.html',
  styleUrl: './chart-engine.component.scss'
})
export class ChartEngineComponent {
  // ticker: WritableSignal<string | undefined> = signal(undefined);
  //
  // availableMetrics: string[] = ['revenue', 'netIncome', 'grossProfit',
  //   'totalAssets', 'totalLiabilities', 'totalStockholdersEquity',
  //   'freeCashFlow', 'stockBasedCompensation', 'cashAtEndOfPeriod'];
  //

  title: WritableSignal<string | undefined> = signal(undefined);
  xAxis: WritableSignal<string[] | undefined> = signal(undefined);
  yData: WritableSignal<number[] | undefined> = signal(undefined);
  yLabel: WritableSignal<string | undefined> = signal(undefined);

  modalService = inject(NzModalService);

  openAdHocChartModal() {
    const modalRef = this.modalService.create({
      nzTitle: 'Create Chart for Ad-Hoc Analysis',
      nzContent: AdHocChartModalComponent,
      nzWidth: '600px',
      nzData: {
        // parent -> modal
        chartTitle: this.title(),
        chartXValues: this.xAxis(),
        chartYValues: this.yData(),
        chartYLabel: this.yLabel()
      },
      nzFooter: null
    });

    // modal -> parent
    // listens for modal close event, which 'result' is { chartTitle: [...], ... }
    modalRef.afterClose.subscribe({
      next: result => {
        if (result) {
          this.title.set(result.chartTitle);
          this.xAxis.set(result.chartXAxis);
          this.yData.set(result.chartYVals);
          this.yLabel.set(result.chartYLabel);
        }
      }
    });
  }
}
