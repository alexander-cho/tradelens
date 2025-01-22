import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { FeedComponent } from './features/feed/feed.component';
import { LandingComponent } from './features/landing/landing.component';

export const routes: Routes = [
  {path: '', component:HomeComponent},
  {path: 'landing', component:LandingComponent},
  {path: 'feed', component:FeedComponent},
];
