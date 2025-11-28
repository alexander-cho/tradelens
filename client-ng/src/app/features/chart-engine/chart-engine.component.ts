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

  title: WritableSignal<string | undefined> = signal('Gross Profit');
  xAxis: WritableSignal<string[] | undefined> = signal(["Q1 2019", "Q2 2019", "Q3 2019", "Q4 2019", "Q1 2020", "Q2 2020", "Q3 2020", "Q4 2020", "Q1 2021", "Q2 2021", "Q3 2021", "Q4 2021", "Q1 2022", "Q2 2022", "Q3 2022", "Q4 2022", "Q1 2023", "Q2 2023", "Q3 2023", "Q4 2023", "Q1 2024", "Q2 2024", "Q3 2024", "Q4 2024", "Q1 2025", "Q2 2025", "Q3 2025"]);
  yData: WritableSignal<number[] | undefined> = signal([852765000, 869000000, 882000000, 901000000, 918000000, 905000000, 927000000, 951000000, 970000000, 962000000, 995000000, 1018000000, 1032000000, 1045000000, 1067000000, 1083000000, 1109000000, 1124000000, 1116000000, 1153000000, 1189000000, 1207000000, 1194000000, 1238000000, 1286000000, 1329000000, 1387000000]);
  yLabel: WritableSignal<string | undefined> = signal('Gross Profit');

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
