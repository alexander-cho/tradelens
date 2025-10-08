import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { CongressService } from '../../core/services/congress.service';
import { CongressTrades } from '../../shared/models/fmp';
import { NzTableComponent } from 'ng-zorro-antd/table';
import { NzRadioComponent, NzRadioGroupComponent } from 'ng-zorro-antd/radio';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-congress',
  imports: [
    NzTableComponent,
    NzRadioGroupComponent,
    NzRadioComponent,
    FormsModule
  ],
  templateUrl: './congress.component.html',
  styleUrl: './congress.component.scss'
})
export class CongressComponent implements OnInit {
  congressService = inject(CongressService);

  chamber: WritableSignal<string> = signal("house");
  congressTrades: WritableSignal<CongressTrades[] | undefined> = signal(undefined);

  ngOnInit() {
    this.getCongressionalTrades();
  }

  onChamberSelectChange(event: string) {
    this.chamber.set(event);
    this.getCongressionalTrades();
  }

  getCongressionalTrades() {
    this.congressService.getCongressionalTrades(this.chamber()).subscribe({
      next: response => this.congressTrades.set(response),
      error: err => console.log(err)
    });
  };
}
