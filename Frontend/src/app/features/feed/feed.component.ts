import { Component, inject, OnInit } from '@angular/core';
import { FeedService } from '../../core/services/feed.service';
import { Post } from '../../shared/models/post';
import { PostComponent } from './post/post.component';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component'
import { MatDialog } from '@angular/material/dialog';
import { NavbarComponent } from '../../layout/navbar/navbar.component';
import { MatMenuModule } from '@angular/material/menu';
import { MatListOption, MatSelectionList, MatSelectionListChange } from '@angular/material/list';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Pagination } from '../../shared/models/pagination';
import { FeedParams } from '../../shared/models/feedParams';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-feed',
  imports: [PostComponent, NavbarComponent, MatMenuModule, MatSelectionList, MatListOption, MatPaginator, FormsModule],
  templateUrl: './feed.component.html',
  styleUrl: './feed.component.scss'
})
export class FeedComponent implements OnInit {
  feedService = inject(FeedService);
  dialogService = inject(MatDialog);

  feedParams = new FeedParams();

  posts?: Pagination<Post>;

  selectedTickers: string[] = [];
  selectedSentiments: string[] = [];

  postsPerPage = [5, 10, 15, 20]

  sortOptions = [
    {name: 'Default', value: ''},
    {name: 'Newest First', value: 'latest'},
    {name: 'Oldest First', value: 'earliest'}
  ]

  getPosts() {
    this.feedService.getPosts(this.feedParams).subscribe({
      next: response => this.posts = response,
      error: error => console.log(error)
    });
  }

  ngOnInit() {
    this.feedService.getTickers();
    this.feedService.getSentiments();
    this.getPosts();
  }

  onSearchChange() {
    this.feedParams.pageNumber = 1;
    this.getPosts();
  }

  handlePageEvent(event: PageEvent) {
    this.feedParams.pageSize = event.pageSize;
    this.feedParams.pageNumber = event.pageIndex + 1;
    this.getPosts();
  }

  onSortChange(event: MatSelectionListChange) {
    const selectedOption = event.options[0];
    if (selectedOption) {
      this.feedParams.sort = selectedOption.value;
      this.getPosts();
    }
  }

  openFiltersDialog() {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: '500px',
      maxHeight: '600px',
      data: {
        selectedTickers: this.feedParams.tickers,
        selectedSentiments: this.feedParams.sentiments
      }
    });

    dialogRef.afterClosed().subscribe({
      next: result => {
        if (result) {
          console.log(result);
          this.feedParams.tickers = result.selectedTickers;
          this.feedParams.sentiments = result.selectedSentiments;
          console.log(this.feedParams);
          this.getPosts();
        }
      }
    });
  }
}
