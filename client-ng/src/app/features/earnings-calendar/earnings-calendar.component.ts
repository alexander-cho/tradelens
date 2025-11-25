import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { EarningsCalendarService } from '../../core/services/earnings-calendar.service';
import { EarningsCalendarModel } from '../../shared/models/earnings-calendar-model';
import { NzCardComponent } from 'ng-zorro-antd/card';

@Component({
  selector: 'app-earnings-calendar',
  imports: [
    NzCardComponent
  ],
  templateUrl: './earnings-calendar.component.html',
  styleUrl: './earnings-calendar.component.scss'
})
export class EarningsCalendarComponent implements OnInit {
  private earningsCalendarService = inject(EarningsCalendarService);

  protected from: WritableSignal<string> = signal("2025-11-01");
  protected to: WritableSignal<string> = signal("2025-11-30");
  protected earningsCalendar: WritableSignal<EarningsCalendarModel[] | undefined> = signal(undefined);

  ngOnInit() {
    this.getEarningsCalendar();
  }

  private getEarningsCalendar() {
    this.earningsCalendarService.getCongressionalTrades(this.from(), this.to()).subscribe({
      next: response => this.earningsCalendar.set(response),
      error: err => console.log(err)
    });
  }
}
