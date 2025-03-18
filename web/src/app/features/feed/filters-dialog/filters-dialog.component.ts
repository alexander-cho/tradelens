import { Component, inject } from '@angular/core';
import { FeedService } from '../../../core/services/feed.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatDivider } from '@angular/material/divider';
import { MatListOption, MatSelectionList } from '@angular/material/list';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-filters-dialog',
  imports: [
    MatDivider,
    MatSelectionList,
    MatListOption,
    FormsModule
  ],
  // https://stackoverflow.com/questions/78501478/resolving-nullinjectorerror-no-provider-for-matdialogref-in-angular-component
  // https://stackoverflow.com/questions/79404902/this-dialogref-close-is-not-a-function-angular-19
  // providers: [
  //   { provide: MAT_DIALOG_DATA, useValue: {} }
  // ],
  // quite strange, if I provide the above, the selections get cleared when I open the dialog again
  templateUrl: './filters-dialog.component.html',
  styleUrl: './filters-dialog.component.scss'
})
export class FiltersDialogComponent {
  feedService = inject(FeedService);
  dialogRef = inject(MatDialogRef<FiltersDialogComponent>);

  // access data passed into dialog
  data = inject(MAT_DIALOG_DATA);

  selectedTickers: string[] = this.data.selectedTickers;
  selectedSentiments: string[] = this.data.selectedSentiments

  applyFilters() {
    this.dialogRef.close({
      selectedTickers: this.selectedTickers,
      selectedSentiments: this.selectedSentiments
    });
    console.log(this.selectedTickers, this.selectedSentiments);
  }
}
