import { Component, inject, OnInit } from '@angular/core';
import { FeedService } from '../../../core/services/feed.service';
import { DividerModule } from 'primeng/divider';
import { Button } from 'primeng/button';
import { ListboxModule } from 'primeng/listbox';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-filters-dialog',
  imports: [
    DividerModule,
    Button,
    ListboxModule,
    FormsModule
  ],
  templateUrl: './filters-dialog.component.html',
  styleUrl: './filters-dialog.component.scss'
})
export class FiltersDialogComponent implements OnInit {
  feedService = inject(FeedService);
  private dialogRef = inject(DynamicDialogRef<FiltersDialogComponent>);

  // access data being passed through dialog
  config = inject(DynamicDialogConfig);

  // ListBox expects array of objects, tickers/sentiments is an array of strings
  tickerOptions: object[] = [];
  sentimentOptions: object[] = [];

  userSelectedTickers = this.config.data.selectedTickers;
  userSelectedSentiments = this.config.data.selectedSentiments;

  ngOnInit() {
    this.convertToObject();
  }

  convertToObject() {
    this.tickerOptions = this.feedService.tickers.map(ticker => ({
      label: ticker,
      value: ticker
    }));

    this.sentimentOptions = this.feedService.sentiments.map(sentiment => ({
      label: sentiment,
      value: sentiment
    }));
  }

  // function to apply selected filters and exit the dialog
  applyFilters() {
    this.dialogRef.close({
      selectedTickers: this.userSelectedTickers,
      selectedSentiments: this.userSelectedSentiments
    })
  }
}
