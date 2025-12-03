import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { MacroService } from '../../core/services/macro.service';
import { SeriesObservations } from '../../shared/models/macro';
import { NzCardComponent } from 'ng-zorro-antd/card';
import { MacroChartComponent } from '../../shared/components/macro-chart/macro-chart.component';

@Component({
  selector: 'app-macro',
  imports: [
    NzCardComponent,
    MacroChartComponent
  ],
  templateUrl: './macro.component.html',
  styleUrl: './macro.component.scss'
})
export class MacroComponent implements OnInit {
  private macroService = inject(MacroService);

  protected seriesObservations: WritableSignal<SeriesObservations | undefined> = signal(undefined);
  protected seriesId: WritableSignal<string> = signal('BOGZ1FL663067003Q');

  ngOnInit() {
    this.getSeriesObservations();
  }

  private getSeriesObservations() {
    this.macroService.getSeriesObservations(this.seriesId()).subscribe({
      next: response => this.seriesObservations.set(response),
      error: err => console.log(err)
    })
  }
}
