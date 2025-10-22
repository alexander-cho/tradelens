import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { LandingComponent } from './features/auth/landing/landing.component';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { FeedComponent } from './features/feed/feed.component';
import { CompaniesComponent } from './features/companies/companies.component';
import { CompanyDashboardComponent } from './features/company-dashboard/company-dashboard.component';
import { CongressComponent } from './features/congress/congress.component';
import { OptionsComponent } from './features/options/options.component';
import { OptionsChainDetailComponent } from './features/options/options-chain-detail/options-chain-detail.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'auth/landing', component: LandingComponent },
  { path: 'login', component: LoginComponent },
  { path: 'auth/register', component: RegisterComponent },
  { path: 'feed', component: FeedComponent },
  { path: 'companies', component: CompaniesComponent },
  { path: 'companies/:ticker', component: CompanyDashboardComponent },
  { path: 'congress', component: CongressComponent },
  { path: 'options', component: OptionsComponent },
  { path: 'options/:ticker/:expiration', component: OptionsChainDetailComponent }
];
