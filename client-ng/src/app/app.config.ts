import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
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
  BankOutline
} from '@ant-design/icons-angular/icons';

registerLocaleData(en);

const icons: IconDefinition[] = [ LockOutline, UserOutline, MenuFoldOutline, MenuUnfoldOutline, FundTwoTone, ReadOutline, AreaChartOutline, BarChartOutline, BookOutline, GlobalOutline, ShopOutline, DollarOutline, BankOutline ];

export const appConfig: ApplicationConfig = {
  providers: [
    provideZonelessChangeDetection(),
    provideRouter(routes, withComponentInputBinding()),
    provideHttpClient(withInterceptors([ authInterceptor ])),
    provideBrowserGlobalErrorListeners(),
    provideNzI18n(en_US),
    provideAnimationsAsync(),
    provideNzIcons(icons)
  ]
};
