import { Component, effect, inject, input, OnInit, signal, WritableSignal } from '@angular/core';
import { Stock } from "../../shared/models/stock";
import { RelatedCompanies } from '../../shared/models/polygon';
import { CompanyDashboardService } from '../../core/services/company-dashboard.service';
import { Router } from '@angular/router';
import { CompanyFundamentalsResponse } from '../../shared/models/fundamentals/company-fundamentals-response';
import { CompanyMetricChartComponent } from '../../shared/components/company-metric-chart/company-metric-chart.component';
import { NzButtonComponent } from 'ng-zorro-antd/button';
import { SelectMetricsModalComponent } from './select-metrics-modal/select-metrics-modal.component';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzColDirective, NzRowDirective } from 'ng-zorro-antd/grid';
import {NzRadioComponent, NzRadioGroupComponent} from 'ng-zorro-antd/radio';
import {FormsModule} from '@angular/forms';
// import { CandlestickChartComponent } from './candlestick-chart/candlestick-chart.component';

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
    // CandlestickChartComponent
  ],
  providers: [NzModalService],
  templateUrl: './company-dashboard.component.html',
  styleUrl: './company-dashboard.component.scss'
})
export class CompanyDashboardComponent implements OnInit {
  // get ticker from url path, as defined in routes
  ticker = input.required<string>();

  companyDashboardService = inject(CompanyDashboardService);
  router = inject(Router);
  modalService = inject(NzModalService);

  relatedCompanies: WritableSignal<RelatedCompanies | undefined> = signal(undefined);
  fundamentalData: WritableSignal<CompanyFundamentalsResponse | undefined> = signal(undefined);
  stock: WritableSignal<Stock | undefined> = signal(undefined);

  period: WritableSignal<string> = signal('quarter');

  // if amount of availableMetrics gets longer, have to change how many selected to initially render
  availableMetrics: string[] = ['revenue', 'netIncome', 'grossProfit',
    'totalAssets', 'totalLiabilities', 'totalStockholdersEquity',
    'freeCashFlow', 'stockBasedCompensation', 'cashAtEndOfPeriod'];
  selectedMetrics: WritableSignal<string[]> = signal(this.availableMetrics);

  ngOnInit() {
    this.companyDashboardService.getStockByTicker(this.ticker()).subscribe({
      next: response => {
        this.stock.set(response);
        this.getRelatedCompanies();
      },
      error: err => {
        console.log(err);
        this.router.navigateByUrl('/companies');
      }
    });
  }

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

  getRelatedCompanies() {
    this.companyDashboardService.getRelatedCompanies(this.ticker()).subscribe({
      next: response => this.relatedCompanies.set(response),
      error: err => console.log(err)
    });
  }

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
