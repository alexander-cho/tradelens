import { Component, computed, inject, OnInit, Signal, signal, WritableSignal } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzModalService } from 'ng-zorro-antd/modal';
import { CompanyDashboardService } from '../../core/services/company-dashboard.service';
import { NzOptionComponent, NzSelectComponent } from 'ng-zorro-antd/select';
import { CHART_ENGINE_COLORS } from '../../shared/utils/barchart-colors';
import { NzButtonComponent } from 'ng-zorro-antd/button';
import { NzRadioComponent, NzRadioGroupComponent } from 'ng-zorro-antd/radio';
import { Chart, ChartTypeRegistry } from 'chart.js/auto';
import { CompanyMetricsService } from '../../core/services/company-metrics.service';
import { CompanyMetricDto } from '../../shared/models/fundamentals/company-metric-dto';
import { KeyValuePipe } from '@angular/common';
import { NzCardComponent } from 'ng-zorro-antd/card';
import { NzMarks, NzSliderComponent } from 'ng-zorro-antd/slider';
import { ValueDataAtEachPeriod } from '../../shared/models/fundamentals/company-fundamentals-response';


@Component({
  selector: 'app-chart-engine',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    NzSelectComponent,
    NzOptionComponent,
    NzButtonComponent,
    NzRadioComponent,
    NzRadioGroupComponent,
    KeyValuePipe,
    NzCardComponent,
    NzSliderComponent
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

  protected selectedChartType: WritableSignal<keyof ChartTypeRegistry | undefined> = signal<keyof ChartTypeRegistry | undefined>(undefined);

  protected selectedColor: WritableSignal<string | undefined> = signal<string | undefined>(undefined);

  protected companyMetricsResponse: WritableSignal<CompanyMetricDto | undefined> = signal<CompanyMetricDto | undefined>(undefined);

  protected interval: WritableSignal<string> = signal('quarterly');

  protected from: WritableSignal<string | undefined> = signal<string | undefined>(undefined);
  protected to: WritableSignal<string | undefined> = signal<string | undefined>(undefined);
  protected fullTimeline: WritableSignal<{ periodEndDates: string[], labels: string[] } | undefined> = signal<{
    periodEndDates: string[],
    labels: string[]
  } | undefined>(undefined);

  protected sliderRange: WritableSignal<number[] | undefined> = signal<number[] | undefined>(undefined);

  protected sliderTimelineMarks: Signal<NzMarks | null> = computed(() => {
    if (!this.fullTimeline()) {
      return null;
    }

    const marks: NzMarks = {};
    this.fullTimeline()?.labels.forEach((label, index) => {
      if (this.interval() == 'quarterly') {
        if (index % 4 == 0) {
          marks[index] = label;
        }
      } else if (this.interval() == 'annual') {
        marks[index] = label;
      }
    });
    // add the last period end date label in case total is not divisible by whichever number defined in the if-statement
    const lastIndex = this.fullTimeline()?.labels.length! - 1;
    marks[lastIndex] = this.fullTimeline()!.labels[lastIndex];
    return marks;
  });

  protected colorsList: Record<string, string> = CHART_ENGINE_COLORS;

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
    // clear available metrics if user changes ticker
    this.selectedMetric.set(undefined);

    // clear timeline if user changes ticker (could I have this encapsulated elsewhere, I probably need to write something
    // similar for when user changes a metric or interval (quarterly/annual)
    this.from.set(undefined);
    this.to.set(undefined);
    this.fullTimeline.set(undefined);
    this.sliderRange.set(undefined);

    this.chart?.destroy();

    this.companyMetricsService.getAllMetricNamesAssociatedWithTicker(this.ticker()!).subscribe({
      next: response => {
        this.availableMetrics.set(response);
        console.log('Metrics for', this.ticker(), ': \n', this.availableMetrics());
      },
      error: err => console.log(err)
    });
  }

  // things to do when user selects a different metric but stays on the same ticker symbol
  protected onMetricChange() {
    this.from.set(undefined);
    this.to.set(undefined);
    this.fullTimeline.set(undefined);
    this.sliderRange.set(undefined);
  }

  // things to do when user selects a different interval on the radio button group
  protected onIntervalChange() {
    this.from.set(undefined);
    this.to.set(undefined);
    this.fullTimeline.set(undefined);
    this.sliderRange.set(undefined);
  }

  protected getDataAndCreateChart() {
    this.companyMetricsService.getAllMetrics(this.ticker(), this.interval(), this.selectedMetric()!, this.from(), this.to()).subscribe({
      next: response => {
        this.companyMetricsResponse.set(response);
        if (!this.fullTimeline()) {
          this.getFullTimeline();
        }
        this.createChart();
        console.log('Metrics data for', this.ticker(), this.selectedMetric(), ': \n', this.companyMetricsResponse());
        console.log('from: ', this.from());
        console.log('to: ', this.to());
      },
      error: err => console.log(err)
    });
  }

  /*
  * Method to get slider timeline values. When only dealing with charting one metric at a time, there's no need to create
  * the master timeline, though the logic for that exists in the ExpandCompanyMetricChartModalComponent.
  * This below implementation is from the aforementioned component as well, for single metric.
  */
  private createTimelineForSingleMetric = (valueDataList: ValueDataAtEachPeriod[]): {
    periodEndDates: string[],
    labels: string[]
  } => {
    const periodEndDates: string[] = [];
    const labels: string[] = [];
    valueDataList.forEach(valueDataPoint => {
      periodEndDates.push(valueDataPoint.periodEndDate);
      let quarterFiscalYear = valueDataPoint.period + ' ' + valueDataPoint.fiscalYear;
      labels.push(quarterFiscalYear);
    });
    return {
      periodEndDates: periodEndDates,
      labels: labels
    }
  }

  private getFullTimeline() {
    if (this.companyMetricsResponse() != null) {
      this.fullTimeline.set(this.createTimelineForSingleMetric(this.companyMetricsResponse()?.data!));
      this.sliderRange.set([0, this.fullTimeline()!.periodEndDates.length - 1]);
    }
  }

  protected onSliderChange(indices: number[]) {
    this.from.set(this.fullTimeline()?.periodEndDates[indices[0]]);
    this.to.set(this.fullTimeline()?.periodEndDates[indices[1]]);
  }

  protected createChart() {
    const dataRead = this.companyMetricsResponse();
    const metricDisplayName = this.companyMetricsResponse()?.metricName;

    if (!metricDisplayName || !dataRead) {
      return;
    }

    this.destroyChart();

    this.chart = new Chart("chart-engine", {
      type: this.selectedChartType()!,
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
            display: false
          }
        }
      },
    });
  }

  destroyChart() {
    this.chart?.destroy();
  }
}
