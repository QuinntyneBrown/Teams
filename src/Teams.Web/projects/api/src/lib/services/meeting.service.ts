import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_CONFIG } from '../api-config';
import {
  CreateMeetingRequest,
  CreateMeetingResult,
  RespondToMeetingRequest,
  RespondToMeetingResult,
  TodayScheduleResult,
  UpcomingMeetingsResult,
} from '../models';

@Injectable({ providedIn: 'root' })
export class MeetingService {
  private readonly http = inject(HttpClient);
  private readonly config = inject(API_CONFIG);

  getTodaySchedule(userId: string, timeZoneId = 'UTC'): Observable<TodayScheduleResult> {
    const params = new HttpParams().set('userId', userId).set('timeZoneId', timeZoneId);
    return this.http.get<TodayScheduleResult>(`${this.config.baseUrl}/api/meetings/today`, { params });
  }

  getUpcomingMeetings(userId: string, count = 3): Observable<UpcomingMeetingsResult> {
    const params = new HttpParams().set('userId', userId).set('count', count);
    return this.http.get<UpcomingMeetingsResult>(`${this.config.baseUrl}/api/meetings/upcoming`, { params });
  }

  createMeeting(request: CreateMeetingRequest): Observable<CreateMeetingResult> {
    return this.http.post<CreateMeetingResult>(`${this.config.baseUrl}/api/meetings`, request);
  }

  respondToMeeting(meetingId: string, request: RespondToMeetingRequest): Observable<RespondToMeetingResult> {
    return this.http.post<RespondToMeetingResult>(`${this.config.baseUrl}/api/meetings/${meetingId}/respond`, request);
  }
}
