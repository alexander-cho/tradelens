import { Component, inject } from '@angular/core';
import { StockService } from '../../../core/services/stock.service';
import { NZ_MODAL_DATA, NzModalRef } from 'ng-zorro-antd/modal';
import { FormsModule } from '@angular/forms';
import { NzButtonComponent } from 'ng-zorro-antd/button';
import { NzOptionComponent, NzSelectComponent } from 'ng-zorro-antd/select';

@Component({
  selector: 'app-filters-modal',
  imports: [
    FormsModule,
    NzButtonComponent,
    NzOptionComponent,
    NzSelectComponent
  ],
  templateUrl: './filters-modal.component.html',
  styleUrl: './filters-modal.component.scss'
})
export class FiltersModalComponent {
  stockService = inject(StockService);
  modalRef = inject(NzModalRef<FiltersModalComponent>);

  // access data passed into dialog
  data = inject(NZ_MODAL_DATA);

  selectedIpoYears: number[] = this.data.selectedIpoYears;
  selectedCountries: string[] = this.data.selectedCountries;
  selectedSectors: string[] = this.data.selectedSectors;

  applyFilters() {
    this.modalRef.close({
      selectedIpoYears: this.selectedIpoYears,
      selectedCountries: this.selectedCountries,
      selectedSectors: this.selectedSectors
    });
    console.log(this.selectedIpoYears, this.selectedCountries, this.selectedSectors);
  }
}
