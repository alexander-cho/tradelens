import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core';
import { provideRouter, withComponentInputBinding, withInMemoryScrolling } from '@angular/router';
import { routes } from './app.routes';
import { en_US, provideNzI18n } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './core/interceptors/auth.interceptor';
import { provideNzIcons } from 'ng-zorro-antd/icon';
import { IconDefinition } from '@ant-design/icons-angular';

import {
  LockOutline,
  UserOutline,
  MenuFoldOutline,
  MenuUnfoldOutline,
  FundTwoTone,
  ReadOutline,
  AreaChartOutline,
  BarChartOutline,
  BookOutline,
  GlobalOutline,
  ShopOutline,
  DollarOutline,
  BankOutline,
  LayoutTwoTone,
  EditOutline,
  RedoOutline
} from '@ant-design/icons-angular/icons';
import { Chart, Filler } from 'chart.js/auto';

registerLocaleData(en);

const icons: IconDefinition[] = [LockOutline, UserOutline, MenuFoldOutline, MenuUnfoldOutline, FundTwoTone, ReadOutline, AreaChartOutline, BarChartOutline, BookOutline, GlobalOutline, ShopOutline, DollarOutline, BankOutline, LayoutTwoTone, EditOutline, RedoOutline];

// global chart color config
Chart.defaults.color = '#B3B3B3';
Chart.defaults.borderColor = 'rgba(59, 59, 59, 0.81)';
Chart.register(Filler);

export const appConfig: ApplicationConfig = {
  providers: [
    provideZonelessChangeDetection(),
    provideRouter(routes, withComponentInputBinding(), withInMemoryScrolling({ scrollPositionRestoration: 'enabled' })),
    provideHttpClient(withInterceptors([authInterceptor])),
    provideBrowserGlobalErrorListeners(),
    provideNzI18n(en_US),
    provideAnimationsAsync(),
    provideNzIcons(icons)
  ]
};
