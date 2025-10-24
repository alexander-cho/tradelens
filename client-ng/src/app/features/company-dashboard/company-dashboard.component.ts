import { Component, effect, inject, input, signal, WritableSignal } from '@angular/core';
import { CompanyDashboardService } from '../../core/services/company-dashboard.service';
import { CompanyFundamentalsResponse } from '../../shared/models/fundamentals/company-fundamentals-response';
import {
  CompanyMetricChartComponent
} from '../../shared/components/company-metric-chart/company-metric-chart.component';
import { NzButtonComponent } from 'ng-zorro-antd/button';
import { SelectMetricsModalComponent } from './select-metrics-modal/select-metrics-modal.component';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzColDirective, NzRowDirective } from 'ng-zorro-antd/grid';
import { NzRadioComponent, NzRadioGroupComponent } from 'ng-zorro-antd/radio';
import { FormsModule } from '@angular/forms';
import { CompanyProfileComponent } from './company-profile/company-profile.component';
import { NzCardComponent } from 'ng-zorro-antd/card';

@Component({
  selector: 'app-company-dashboard',
  imports: [
    CompanyMetricChartComponent,
    NzButtonComponent,
    NzRowDirective,
    NzColDirective,
    NzRadioComponent,
    NzRadioGroupComponent,
    FormsModule,
    CompanyProfileComponent,
    NzCardComponent,
  ],
  providers: [NzModalService],
  templateUrl: './company-dashboard.component.html',
  styleUrl: './company-dashboard.component.scss'
})
export class CompanyDashboardComponent {
  // get ticker from url path, as defined in routes
  ticker = input.required<string>();

  companyDashboardService = inject(CompanyDashboardService);
  modalService = inject(NzModalService);

  fundamentalData: WritableSignal<CompanyFundamentalsResponse | undefined> = signal(undefined);

  period: WritableSignal<string> = signal('quarter');

  // if amount of availableMetrics gets longer, have to change how many selected to initially render
  availableMetrics: string[] = ['revenue', 'netIncome', 'grossProfit',
    'totalAssets', 'totalLiabilities', 'totalStockholdersEquity',
    'freeCashFlow', 'stockBasedCompensation', 'cashAtEndOfPeriod'];
  selectedMetrics: WritableSignal<string[]> = signal(this.availableMetrics);

  // on initial load, the period is set as 'annual' (Yearly) and there will be x amount of pre-selected metrics,
  // 6 or 9, around ones that all companies have in common, e.g. revenue, etc.
  // update metrics reactively
  metricsChangeEffect = effect(() => {
    const changedPeriod = this.period();
    const changedMetricsList = this.selectedMetrics();

    if (changedPeriod || changedMetricsList) {
      this.getUserRequestedCompanyFundamentalData();
    }
  });

  getUserRequestedCompanyFundamentalData() {
    this.companyDashboardService.getCompanyFundamentalData(this.ticker(), this.period(), this.selectedMetrics()).subscribe({
      next: response => {
        this.fundamentalData.set(response);
        console.log('Backend returned:', response.metricData);
      },
      error: err => console.log(err)
    });
  }

  openSelectMetricsModal() {
    const modalRef = this.modalService.create({
      nzTitle: 'Select Metrics',
      nzContent: SelectMetricsModalComponent,
      nzWidth: '600px',
      nzData: {
        // parent -> modal
        availableMetrics: this.availableMetrics,
        selectedMetrics: this.selectedMetrics()
      },
      nzFooter: null
    });

    // modal -> parent
    // listens for modal close event, which 'result' is { selectedMetrics: [...] }
    modalRef.afterClose.subscribe({
      next: result => {
        if (result) {
          this.selectedMetrics.set(result.selectedMetrics);
        }
      }
    });
  }
}
