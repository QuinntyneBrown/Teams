import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { API_CONFIG } from 'api';

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(),
    {
      provide: API_CONFIG,
      useValue: {
        baseUrl: 'http://localhost:5000',
        accessTokenFactory: () => localStorage.getItem('auth_token') ?? '',
      },
    },
  ]
};
