import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// import { NzButtonComponent } from 'ng-zorro-antd/button';
import { NzModalService } from 'ng-zorro-antd/modal';
// import { ChartEngineChartComponent } from './chart-engine-chart/chart-engine-chart.component';
import { CompanyDashboardService } from '../../core/services/company-dashboard.service';
import { NzOptionComponent, NzSelectComponent } from 'ng-zorro-antd/select';
import { CompanyFundamentalsResponse } from '../../shared/models/fundamentals/company-fundamentals-response';
import { BARCHART_COLORS } from '../../shared/utils/barchart-colors';
import { NzButtonComponent } from 'ng-zorro-antd/button';
import { NzRadioComponent, NzRadioGroupComponent } from 'ng-zorro-antd/radio';

// import { NzInputDirective } from 'ng-zorro-antd/input';

@Component({
  selector: 'app-chart-engine',
  imports: [
    FormsModule,
    // NzInputDirective,
    ReactiveFormsModule,
    NzSelectComponent,
    NzOptionComponent,
    NzButtonComponent,
    NzRadioComponent,
    NzRadioGroupComponent
  ],
  providers: [NzModalService],
  templateUrl: './chart-engine.component.html',
  styleUrl: './chart-engine.component.scss'
})
export class ChartEngineComponent implements OnInit {
  protected ticker: WritableSignal<string | undefined> = signal<string | undefined>(undefined);

  protected availableCompanies: WritableSignal<string[] |  undefined> = signal<string[] |  undefined>(undefined);
  protected availableMetrics: WritableSignal<string[] |  undefined> = signal<string[] |  undefined>(undefined);
  protected selectedMetric: WritableSignal<string | undefined> = signal<string | undefined>(undefined);
  // protected selectedMetrics: WritableSignal<string[] | undefined> = signal<string[] | undefined>(undefined);

  protected selectedChartType: WritableSignal<string | undefined> = signal<string | undefined>(undefined);

  protected selectedColor: WritableSignal<string | undefined> = signal<string | undefined>(undefined);

  protected metricData: WritableSignal<CompanyFundamentalsResponse | undefined> = signal<CompanyFundamentalsResponse | undefined>(undefined);

  protected interval: WritableSignal<string> = signal('quarterly');

  private companyDashboardService = inject(CompanyDashboardService);

  protected colorsList = BARCHART_COLORS;

  ngOnInit() {
    this.getAvailableCompanies();
  }

  private getAvailableCompanies() {
    this.companyDashboardService.getAvailableCompanies().subscribe({
      next: response => this.availableCompanies.set(response),
      error: err => console.log(err)
    });
  }

  protected getListOfAvailableMetricsTemp() {
    this.companyDashboardService.getAllMetricNamesAssociatedWithTicker(this.ticker()!).subscribe({
      next: response => {
        this.availableMetrics.set(response);
        console.log('Metrics for', this.ticker(), ': \n',this.availableMetrics());
      },
      error: err => console.log(err)
    });
  }

  protected getDataForSelectedMetric() {
    this.companyDashboardService.getAllMetrics(this.ticker(), 'quarterly', this.selectedMetric()!).subscribe({
      next: response => {
        this.metricData.set(response);
        console.log('Metrics data for', this.ticker(), this.selectedMetric(), ': \n', this.metricData());
      },
      error: err => console.log(err)
    });
  }

  protected getSelectedColor() {
    console.log(this.selectedColor());
  }

  protected getSelectedChartType() {
    console.log(this.selectedChartType());
  }
}
