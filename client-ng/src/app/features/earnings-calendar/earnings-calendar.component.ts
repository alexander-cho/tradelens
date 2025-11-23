import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { EarningsCalendarService } from '../../core/services/earnings-calendar.service';
import { EarningsCalendarModel } from '../../shared/models/earnings-calendar-model';

@Component({
  selector: 'app-earnings-calendar',
  imports: [],
  templateUrl: './earnings-calendar.component.html',
  styleUrl: './earnings-calendar.component.scss'
})
export class EarningsCalendarComponent implements OnInit {
  earningsCalendarService = inject(EarningsCalendarService);

  from: string = "2025-11-01";
  to: string = "2025-11-30";

  protected earningsCalendar: WritableSignal<EarningsCalendarModel[] | undefined> = signal(undefined);

  ngOnInit() {
    this.getEarningsCalendar();
  }

  getEarningsCalendar() {
    this.earningsCalendarService.getCongressionalTrades(this.from, this.to).subscribe({
      next: response => this.earningsCalendar.set(response),
      error: err => console.log(err)
    });
  }
}
