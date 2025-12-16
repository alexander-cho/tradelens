import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { MacroService } from '../../core/services/macro.service';
import { SeriesObservations } from '../../shared/models/macro';
import { NzCardComponent } from 'ng-zorro-antd/card';
import { Chart } from 'chart.js/auto';
import { MACRO_SERIES } from '../../shared/utils/macro-series';
import { NzOptionComponent, NzSelectComponent } from 'ng-zorro-antd/select';
import { KeyValuePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-macro',
  imports: [
    NzCardComponent,
    NzSelectComponent,
    NzOptionComponent,
    KeyValuePipe,
    FormsModule,
  ],
  templateUrl: './macro.component.html',
  styleUrl: './macro.component.scss'
})
export class MacroComponent implements OnInit {
  private macroService = inject(MacroService);

  protected chart?: Chart;

  protected seriesList: Record<string, string> = MACRO_SERIES;

  protected seriesId: WritableSignal<string> = signal('BOGZ1FL663067003Q');
  protected seriesObservations: WritableSignal<SeriesObservations | undefined> = signal(undefined);

  ngOnInit() {
    this.getSeriesObservationsAndCreateChart();
  }

  protected getSeriesObservationsAndCreateChart() {
    this.macroService.getSeriesObservations(this.seriesId()).subscribe({
      next: response => {
        this.seriesObservations.set(response);
        this.createMacroChart();
      },
      error: err => console.log(err)
    })
  }

  private createMacroChart() {
    const dataRead = this.seriesObservations()?.observations;

    if (!dataRead) {
      return;
    }

    this.chart?.destroy();

    this.chart = new Chart('macroChart', {
      type: 'line',
      data: {
        labels: dataRead.map(x => x.date),
        datasets: [
          {
            label: 'Margin',
            data: dataRead.map(x => x.value),
            borderWidth: 1,
            borderColor: 'rgba(0, 250, 0, 0.7)',
            backgroundColor: 'rgba(0, 250, 0, 0.7)'
          }
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        scales: {
          x: {},
          y: {
            beginAtZero: true,
          },
        },
        plugins: {
          title: {
            text: ``,
            display: false
          }
        }
      }
    });
  }
}
