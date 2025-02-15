import { Component, inject } from '@angular/core';
import { MatDivider } from '@angular/material/divider';
import { MatListOption, MatSelectionList } from '@angular/material/list';
import { FormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { StockService } from '../../../core/services/stock.service';

@Component({
  selector: 'app-filters-dialog',
  imports: [
    MatDivider,
    MatSelectionList,
    MatListOption,
    FormsModule
  ],
  templateUrl: './filters-dialog.component.html',
  styleUrl: './filters-dialog.component.scss'
})
export class FiltersDialogComponent {
  stockService = inject(StockService);
  dialogRef = inject(MatDialogRef<FiltersDialogComponent>);

  // access data passed into dialog
  data = inject(MAT_DIALOG_DATA);

  selectedIpoYears: number[] = this.data.selectedIpoYears;
  selectedCountries: string[] = this.data.selectedCountries;
  selectedSectors: string[] = this.data.selectedSectors;

  applyFilters() {
    this.dialogRef.close({
      selectedIpoYears: this.selectedIpoYears,
      selectedCountries: this.selectedCountries,
      selectedSectors: this.selectedSectors
    });
    // console.log(this.selectedIpoYears, this.selectedCountries, this.selectedSectors);
  }
}
