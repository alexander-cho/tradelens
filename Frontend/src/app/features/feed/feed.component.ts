import { Component, inject, model, OnInit } from '@angular/core';
import { FeedService } from '../../core/services/feed.service';
import { Post } from '../../shared/models/post';
import { CardModule } from 'primeng/card';
import { PostComponent } from './post/post.component';
import { DynamicDialogModule, DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { Button, ButtonModule } from 'primeng/button';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-feed',
  imports: [CardModule, PostComponent, Button],
  // need to add dialogService to providers
  providers: [DialogService, DynamicDialogRef],
  templateUrl: './feed.component.html',
  styleUrl: './feed.component.scss'
})
export class FeedComponent implements OnInit {
  feedService = inject(FeedService);
  private dialogService = inject(DialogService);

  // properties to see which tickers/sentiments have been selected, pass to the filters dialog below
  selectedTickers: string[] = [];
  selectedSentiments: string[] = [];
  selectedSort: string = 'id';
  sortOptions: MenuItem[] = [];

  posts: Post[] = [];

  ngOnInit() {
    // this.feedService.getPosts().subscribe({
    //   next: response => this.posts = response.data,
    //   error: error => console.log(error)
    // });
    this.initializeFeed();
    this.sortOptions = [
      {name: 'id', value: 'id'},
      {name: 'Oldest', value: 'earliest'},
      {name: 'Latest', value: 'latest'}
    ]
  }

  initializeFeed() {
    this.feedService.getTickers();
    this.feedService.getSentiments();
    this.feedService.getPosts().subscribe({
      next: response => this.posts = response.data,
      error: error => console.log(error)
    });
  }

  // method to open dialog
  openFiltersDialog() {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      width: '50%',
      data: {
        selectedTickers: this.selectedTickers,
        selectedSentiments: this.selectedSentiments
      }
    });
    // do something upon dialog closure
    dialogRef.onClose.subscribe({
      next: result => {
        if (result) {
          this.selectedTickers = result.selectedTickers;
          this.selectedSentiments = result.selectedSentiments;

          // get the posts using the query string built from user-selected filters
          this.feedService.getPosts(this.selectedTickers, this.selectedSentiments).subscribe({
            next: response => this.posts = response.data,
            error: err => console.log(err)
          })
        }
      }
    });
  }

  protected readonly model = model;
}
