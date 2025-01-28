import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { FeedComponent } from './features/feed/feed.component';
import { LandingComponent } from './features/landing/landing.component';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';

export const routes: Routes = [
  {path: '', component:HomeComponent},
  {path: 'landing', component:LandingComponent},
  {path: 'feed', component:FeedComponent},
  {path: 'auth/login', component:LoginComponent},
  {path: 'auth/register', component:RegisterComponent}
];
