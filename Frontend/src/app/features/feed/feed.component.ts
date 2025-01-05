import { Component, inject, OnInit } from '@angular/core';
import { FeedService } from '../../core/services/feed.service';
import { Post } from '../../shared/models/post';
import { CardModule } from 'primeng/card';
import { PostComponent } from './post/post.component';

@Component({
  selector: 'app-feed',
  imports: [CardModule, PostComponent],
  templateUrl: './feed.component.html',
  styleUrl: './feed.component.scss'
})
export class FeedComponent implements OnInit {
  feedService = inject(FeedService);

  posts: Post[] = [];

  ngOnInit() {
    // this.feedService.getPosts().subscribe({
    //   next: response => this.posts = response.data,
    //   error: error => console.log(error)
    // });
    this.initializeFeed();
  }

  initializeFeed() {
    this.feedService.getTickers();
    this.feedService.getSentiments();
    this.feedService.getPosts().subscribe({
      next: response => this.posts = response.data,
      error: error => console.log(error)
    });
  }
}
