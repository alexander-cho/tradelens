import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// import { NzButtonComponent } from 'ng-zorro-antd/button';
import { NzModalService } from 'ng-zorro-antd/modal';
// import { ChartEngineChartComponent } from './chart-engine-chart/chart-engine-chart.component';
import { CompanyDashboardService } from '../../core/services/company-dashboard.service';
import { NzOptionComponent, NzSelectComponent } from 'ng-zorro-antd/select';
import { BARCHART_COLORS } from '../../shared/utils/barchart-colors';
import { NzButtonComponent } from 'ng-zorro-antd/button';
import { NzRadioComponent, NzRadioGroupComponent } from 'ng-zorro-antd/radio';
import { Chart, ChartTypeRegistry } from 'chart.js/auto';
import { CompanyMetricsService } from '../../core/services/company-metrics.service';
import { CompanyMetricDto } from '../../shared/models/fundamentals/company-metric-dto';

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
  private companyDashboardService: CompanyDashboardService = inject(CompanyDashboardService);
  private companyMetricsService: CompanyMetricsService = inject(CompanyMetricsService);

  protected ticker: WritableSignal<string | undefined> = signal<string | undefined>(undefined);

  protected availableCompanies: WritableSignal<string[] | undefined> = signal<string[] | undefined>(undefined);
  protected availableMetrics: WritableSignal<string[] | undefined> = signal<string[] | undefined>(undefined);
  protected selectedMetric: WritableSignal<string | undefined> = signal<string | undefined>(undefined);
  // protected selectedMetrics: WritableSignal<string[] | undefined> = signal<string[] | undefined>(undefined);

  protected selectedChartType: WritableSignal<string | undefined> = signal<string | undefined>(undefined);

  protected selectedColor: WritableSignal<string | undefined> = signal<string | undefined>(undefined);

  protected companyMetricsResponse: WritableSignal<CompanyMetricDto | undefined> = signal<CompanyMetricDto | undefined>(undefined);

  protected interval: WritableSignal<string> = signal('quarterly');

  protected colorsList: string[] = BARCHART_COLORS;

  protected chart?: Chart;

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
    this.companyMetricsService.getAllMetricNamesAssociatedWithTicker(this.ticker()!).subscribe({
      next: response => {
        this.availableMetrics.set(response);
        console.log('Metrics for', this.ticker(), ': \n', this.availableMetrics());
      },
      error: err => console.log(err)
    });
  }

  protected getDataForSelectedMetric() {
    this.companyMetricsService.getAllMetrics(this.ticker(), 'quarterly', this.selectedMetric()!).subscribe({
      next: response => {
        this.companyMetricsResponse.set(response);
        console.log('Metrics data for', this.ticker(), this.selectedMetric(), ': \n', this.companyMetricsResponse());
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

  protected createChart() {
    const dataRead = this.companyMetricsResponse();
    const metricDisplayName = this.companyMetricsResponse()?.metricName;
    const selectedChartType: string | undefined = this.selectedChartType();

    if (!metricDisplayName || !dataRead) {
      return;
    }

    this.chart?.destroy();

    this.chart = new Chart("chart-engine", {
      // type: (selectedChartType: keyof ChartTypeRegistry): ChartTypeRegistry,
      type: 'line',
      data: {
        labels: dataRead.data.map(x => x.period + ' ' + x.fiscalYear),
        datasets: [
          {
            label: metricDisplayName,
            data: dataRead.data.map(x => x.value),
            // borderWidth: 1,
            borderColor: this.selectedColor(),
            backgroundColor: this.selectedColor(),
          }
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        scales: {
          x: {},
          y: {
            beginAtZero: true
          },
        },
        plugins: {
          title: {
            text: metricDisplayName,
            display: true
          }
        }
      },
    });
  }

  destroyChart() {
    this.chart?.destroy();
  }
}
