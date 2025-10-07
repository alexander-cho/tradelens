import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { CongressService } from '../../core/services/congress.service';
import { CongressTrades } from '../../shared/models/fmp';
import { NzTableComponent } from 'ng-zorro-antd/table';

@Component({
  selector: 'app-congress',
  imports: [
    NzTableComponent
  ],
  templateUrl: './congress.component.html',
  styleUrl: './congress.component.scss'
})
export class CongressComponent implements OnInit {
  congressService = inject(CongressService);

  houseTrades: WritableSignal<CongressTrades[] | undefined> = signal(undefined);
  senateTrades: WritableSignal<CongressTrades[] | undefined> = signal(undefined);

  ngOnInit() {
    this.getHouseTrades();
    this.getSenateTrades();
  }

  getHouseTrades() {
    this.congressService.getHouseTrades().subscribe({
      next: response => this.houseTrades.set(response),
      error: err => console.log(err)
    });
  }

  getSenateTrades() {
    this.congressService.getSenateTrades().subscribe({
      next: response => this.senateTrades.set(response),
      error: err => console.log(err)
    });
  }
}
