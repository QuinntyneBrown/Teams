import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_CONFIG } from '../api-config';
import { DashboardResult } from '../models';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  private readonly http = inject(HttpClient);
  private readonly config = inject(API_CONFIG);

  getDashboard(userId: string, timeZoneId = 'UTC'): Observable<DashboardResult> {
    const params = new HttpParams().set('timeZoneId', timeZoneId);
    return this.http.get<DashboardResult>(`${this.config.baseUrl}/api/dashboard/${userId}`, { params });
  }
}
