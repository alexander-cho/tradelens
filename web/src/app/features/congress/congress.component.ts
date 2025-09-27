import { Component, inject, OnInit } from '@angular/core';
import { CongressService } from '../../core/services/congress.service';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { CongressTrades } from '../../shared/models/fmp';
// import { MatTab, MatTabGroup } from '@angular/material/tabs';

@Component({
  selector: 'app-congress',
  imports: [NavbarComponent,
    // MatTabGroup,
    // MatTab
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
