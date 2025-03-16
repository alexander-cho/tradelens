import { Component, input } from '@angular/core';
import { Post } from '../../../shared/models/post';

@Component({
  selector: 'app-post',
  imports: [],
  templateUrl: './post.component.html',
  styleUrl: './post.component.scss'
})
export class PostComponent {
  // access posts that need to be passed down from feed component
  // post is optional, since upon creation of this component, we will not have the post
  // check for existence in template
  // receive data from parent component, feed.
  post = input.required<Post>();
}
