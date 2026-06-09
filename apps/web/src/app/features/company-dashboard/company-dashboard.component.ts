import { Component, inject, input, OnInit, signal, WritableSignal } from '@angular/core';
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
import { NzIconDirective } from 'ng-zorro-antd/icon';

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
    NzIconDirective,
  ],
  providers: [NzModalService],
  templateUrl: './company-dashboard.component.html',
  styleUrl: './company-dashboard.component.scss'
})
export class CompanyDashboardComponent implements OnInit {
  // get ticker from url path, as defined in routes
  public ticker = input.required<string>();

  private companyDashboardService = inject(CompanyDashboardService);
  private modalService = inject(NzModalService);

  protected fundamentalData: WritableSignal<CompanyFundamentalsResponse | undefined> = signal(undefined);

  // do not run metricsChangeEffect as soon as component initializes
  // private shouldRunMetricsChangeEffect: WritableSignal<boolean> = signal(false);

  // get list of companies with available metrics to know which modals/metrics, etc. to show
  protected availableCompanies: WritableSignal<string[] | undefined> = signal(undefined);

  protected period: WritableSignal<string> = signal('quarter');

  // NEWER WAY TO GET METRICS; period => interval
  protected interval: WritableSignal<string> = signal('quarterly');
  // NEWER WAY TO GET METRICS

  // if amount of availableMetrics gets longer, have to change how many selected to initially render
  protected availableMetricsFmp: WritableSignal<string[]> = signal(['revenue', 'netIncome', 'grossProfit',
    'totalAssets', 'totalLiabilities', 'totalStockholdersEquity',
    'freeCashFlow', 'stockBasedCompensation', 'cashAtEndOfPeriod']);
  protected selectedMetricsFmp: WritableSignal<string[]> = signal(this.availableMetricsFmp());

  // For example, SOFI, where we get the metrics from the DB
  protected availableMetrics: WritableSignal<string[] | undefined> = signal(undefined);
  protected selectedMetrics: WritableSignal<string[]> = signal(['Revenue', 'NetIncome', 'OperatingExpenses',
    'TotalLiabilities', 'CashAndDebt', 'SharesOutstanding',
    'AdjustedEbitda', 'TotalStockholdersEquity', 'TotalAssets']);

  ngOnInit() {
    // fetch metrics data AFTER we know which companies are available, so that we know whether to fetch from DB or FMP API
    this.companyDashboardService.getAvailableCompanies().subscribe({
      next: response => {
        this.availableCompanies.set(response);
        this.getUserRequestedCompanyFundamentalData();
      },
      error: err => console.log(err)
    });
  }

  protected getUserRequestedCompanyFundamentalData() {
    const availableCompanies: string[] | undefined = this.availableCompanies();
    if (availableCompanies != null && availableCompanies.includes(this.ticker())) {
      this.companyDashboardService.getParentMetricsAssociatedWithTicker(this.ticker()).subscribe({
        next: response => {
          this.availableMetrics.set(response);
        },
        error: err => console.log(err)
      });

      this.companyDashboardService.getCompanyMetricsGroupedByParent(this.ticker(), this.interval(), this.selectedMetrics()).subscribe({
        next: response => {
          this.fundamentalData.set(response);
        },
        error: err => console.log(err)
      });
    } else if (!availableCompanies?.includes(this.ticker())) {
      this.companyDashboardService.getCompanyFundamentalData(this.ticker(), this.period(), this.selectedMetricsFmp()).subscribe({
        next: response => {
          this.fundamentalData.set(response);
        },
        error: err => console.log(err)
      });
    }
  }

  protected openSelectMetricsModalFmp() {
    const modalRef = this.modalService.create({
      nzTitle: 'Select Metrics',
      nzContent: SelectMetricsModalComponent,
      nzWidth: '1000px',
      nzData: {
        // parent -> modal
        availableMetrics: this.availableMetricsFmp,
        selectedMetrics: this.selectedMetricsFmp()
      },
      nzFooter: null
    });

    // modal -> parent
    // listens for modal close event, which 'result' is { selectedMetrics: [...] }
    modalRef.afterClose.subscribe({
      next: result => {
        if (result) {
          this.selectedMetricsFmp.set(result.selectedMetrics);
          this.getUserRequestedCompanyFundamentalData();
        }
      }
    });
  }

  protected openSelectMetricsModal() {
    const modalRef = this.modalService.create({
      nzTitle: 'Select Metrics',
      nzContent: SelectMetricsModalComponent,
      nzWidth: '1000px',
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
          this.getUserRequestedCompanyFundamentalData();
        }
      }
    });
  }

  protected resetCharts() {
    this.selectedMetrics.set(['Revenue', 'NetIncome', 'OperatingExpenses',
      'TotalLiabilities', 'CashAndDebt', 'SharesOutstanding',
      'AdjustedEbitda', 'TotalStockholdersEquity', 'TotalAssets']);
    this.getUserRequestedCompanyFundamentalData()
  }

  protected resetChartsFmp() {
    this.selectedMetricsFmp.set(['revenue', 'netIncome', 'grossProfit',
      'totalAssets', 'totalLiabilities', 'totalStockholdersEquity',
      'freeCashFlow', 'stockBasedCompensation', 'cashAtEndOfPeriod']);
    this.getUserRequestedCompanyFundamentalData();
  }
}
