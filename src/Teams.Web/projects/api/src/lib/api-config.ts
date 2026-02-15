import { InjectionToken } from '@angular/core';

export interface ApiConfig {
  baseUrl: string;
  accessTokenFactory?: () => string | Promise<string>;
}

export const API_CONFIG = new InjectionToken<ApiConfig>('API_CONFIG');
