import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { FeedService } from '../../core/services/feed.service';
import { Post } from '../../shared/models/post';
import { PostComponent } from './post/post.component';
import { Pagination } from '../../shared/models/pagination';
import { FeedParams } from '../../shared/models/feedParams';
import { FormsModule } from '@angular/forms';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzInputDirective } from 'ng-zorro-antd/input';
import { NzButtonComponent } from 'ng-zorro-antd/button';
import { NzModalService } from 'ng-zorro-antd/modal';
import { FiltersModalComponent } from './filters-modal/filters-modal.component';
import { NzDropDownDirective, NzDropdownMenuComponent } from 'ng-zorro-antd/dropdown';
import { NzMenuDirective, NzMenuItemComponent } from 'ng-zorro-antd/menu';
import { debounceTime, Observable } from 'rxjs';
import { toObservable } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-feed',
  imports: [
    PostComponent,
    FormsModule,
    NzPaginationModule,
    NzInputDirective,
    NzButtonComponent,
    NzDropdownMenuComponent,
    NzMenuDirective,
    NzMenuItemComponent,
    NzDropDownDirective,
  ],
  providers: [NzModalService],
  templateUrl: './feed.component.html',
  styleUrl: './feed.component.scss'
})
export class FeedComponent implements OnInit {
  feedService = inject(FeedService);
  modalService = inject(NzModalService);

  searchTerm: WritableSignal<string> = signal<string>("");
  debouncedSearch: Observable<string> = toObservable(this.searchTerm).pipe(
    debounceTime(1000)
  );

  feedParams = new FeedParams();
  posts?: Pagination<Post>;
  postsPerPage = [5, 10, 15, 20];
  sortOptions = [
    { name: 'Default', value: '' },
    { name: 'Newest First', value: 'latest' },
    { name: 'Oldest First', value: 'earliest' }
  ];

  ngOnInit() {
    this.feedService.getTickers();
    this.feedService.getSentiments();
    this.getPosts();

    this.debouncedSearch.subscribe(() => {
      this.feedParams.pageNumber = 1;
      this.feedParams.search = this.searchTerm();
      this.getPosts();
    });
  }

  getPosts() {
    this.feedService.getPosts(this.feedParams).subscribe({
      next: response => this.posts = response,
      error: error => console.log(error)
    });
  }

  handlePageIndexChangeEvent(event: number) {
    this.feedParams.pageNumber = event;
    this.getPosts();
  }

  handlePageSizeChangeEvent(event: number) {
    // https://github.com/NG-ZORRO/ng-zorro-antd/issues/5695
    this.feedParams.pageSize = event;
    this.feedParams.pageNumber = 1;
    this.getPosts();
  }

  onSortChange(value: string) {
    this.feedParams.sort = value;
    this.feedParams.pageNumber = 1;
    this.getPosts();
  }

  openFiltersDialog() {
    const modalRef = this.modalService.create({
      nzTitle: 'Filters',
      nzContent: FiltersModalComponent,
      nzWidth: '500px',
      nzData: {
        selectedTickers: this.feedParams.tickers,
        selectedSentiments: this.feedParams.sentiments
      },
      nzFooter: null
    });

    modalRef.afterClose.subscribe({
      next: result => {
        if (result) {
          this.feedParams.tickers = result.selectedTickers;
          this.feedParams.sentiments = result.selectedSentiments;
          this.feedParams.pageNumber = 1;
          this.getPosts();
        }
      }
    });
  }

  resetParams() {
    this.feedParams.tickers = [];
    this.feedParams.sentiments = [];
    this.feedParams.pageNumber = 1;
    this.feedParams.search = '';
    this.feedParams.sort = '';
    this.getPosts();
  }
}
