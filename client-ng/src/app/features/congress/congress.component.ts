import { Component, inject, OnInit } from '@angular/core';
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

  houseTrades?: CongressTrades[];
  senateTrades?: CongressTrades[];

  ngOnInit() {
    this.getHouseTrades();
    this.getSenateTrades();
  }

  getHouseTrades() {
    this.congressService.getHouseTrades().subscribe({
      next: response => {
        this.houseTrades = response;
      },
      error: err => console.log(err)
    });
  }

  getSenateTrades() {
    this.congressService.getSenateTrades().subscribe({
      next: response => {
        this.senateTrades = response;
      },
      error: err => console.log(err)
    });
  }
}
