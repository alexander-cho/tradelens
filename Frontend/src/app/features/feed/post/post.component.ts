import { Component, Input } from '@angular/core';
import { Post } from '../../../shared/models/post';
import { Card } from 'primeng/card';

@Component({
  selector: 'app-post',
  imports: [Card],
  templateUrl: './post.component.html',
  styleUrl: './post.component.scss'
})
export class PostComponent {
  // access posts that need to be passed down from feed component
  // post is optional, since upon creation of this component, we will not have the post
  // check for existence in template
  @Input() post?: Post;
}
