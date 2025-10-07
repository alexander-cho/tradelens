import { Component, inject } from '@angular/core';
import { FeedService } from '../../../core/services/feed.service';
import { NZ_MODAL_DATA, NzModalRef } from 'ng-zorro-antd/modal';
import { FormsModule } from '@angular/forms';
import { NzButtonComponent } from 'ng-zorro-antd/button';
import { NzOptionComponent, NzSelectComponent } from 'ng-zorro-antd/select';

@Component({
  selector: 'app-filters-modal',
  imports: [
    FormsModule,
    NzButtonComponent,
    NzSelectComponent,
    NzOptionComponent
  ],
  // https://stackoverflow.com/questions/78501478/resolving-nullinjectorerror-no-provider-for-matdialogref-in-angular-component
  // https://stackoverflow.com/questions/79404902/this-dialogref-close-is-not-a-function-angular-19
  // providers: [
  //   { provide: MAT_DIALOG_DATA, useValue: {} }
  // ],
  // quite strange, if I provide the above, the selections get cleared when I open the dialog again
  templateUrl: './filters-modal.component.html',
  styleUrl: './filters-modal.component.scss'
})
export class FiltersModalComponent {
  feedService = inject(FeedService);
  modalRef = inject(NzModalRef<FiltersModalComponent>);

  // access data passed into dialog
  data = inject(NZ_MODAL_DATA);

  selectedTickers: string[] = this.data.selectedTickers;
  selectedSentiments: string[] = this.data.selectedSentiments

  applyFilters() {
    this.modalRef.close({
      selectedTickers: this.selectedTickers,
      selectedSentiments: this.selectedSentiments
    });
    console.log(this.selectedTickers, this.selectedSentiments);
  }
}
