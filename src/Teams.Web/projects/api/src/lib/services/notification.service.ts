import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_CONFIG } from '../api-config';
import { ActivityFeedItemDto, PaginatedNotificationsDto } from '../models';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private readonly http = inject(HttpClient);
  private readonly config = inject(API_CONFIG);

  getNotifications(userId: string, pageSize = 20, page = 0): Observable<PaginatedNotificationsDto> {
    const params = new HttpParams().set('pageSize', pageSize).set('page', page);
    return this.http.get<PaginatedNotificationsDto>(`${this.config.baseUrl}/api/notifications/${userId}`, { params });
  }

  markAsRead(notificationId: string, userId: string): Observable<boolean> {
    const params = new HttpParams().set('userId', userId);
    return this.http.post<boolean>(`${this.config.baseUrl}/api/notifications/${notificationId}/read`, null, { params });
  }

  getActivityFeed(count = 10): Observable<ActivityFeedItemDto[]> {
    const params = new HttpParams().set('count', count);
    return this.http.get<ActivityFeedItemDto[]>(`${this.config.baseUrl}/api/notifications/activity`, { params });
  }
}
