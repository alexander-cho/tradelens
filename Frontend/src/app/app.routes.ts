import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { LandingComponent } from './features/auth/landing/landing.component';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { FeedComponent } from './features/feed/feed.component';
import { CompaniesComponent } from './features/companies/companies.component';

export const routes: Routes = [
  {path: '', component:HomeComponent},
  {path: 'auth/landing', component:LandingComponent},
  {path: 'login', component:LoginComponent},
  {path: 'auth/register', component:RegisterComponent},
  {path: 'feed', component:FeedComponent},
  {path: 'companies', component:CompaniesComponent},
];
