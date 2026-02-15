import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_CONFIG } from '../api-config';
import {
  InviteMemberRequest,
  TeamInvitationDto,
  TeamMemberDto,
  TimeZoneGroupCardDto,
  UpdatePresenceRequest,
} from '../models';

@Injectable({ providedIn: 'root' })
export class TeamService {
  private readonly http = inject(HttpClient);
  private readonly config = inject(API_CONFIG);

  getMembers(status?: string, search?: string): Observable<TeamMemberDto[]> {
    let params = new HttpParams();
    if (status) {
      params = params.set('status', status);
    }
    if (search) {
      params = params.set('search', search);
    }
    return this.http.get<TeamMemberDto[]>(`${this.config.baseUrl}/api/team/members`, { params });
  }

  getTimeZones(): Observable<TimeZoneGroupCardDto[]> {
    return this.http.get<TimeZoneGroupCardDto[]>(`${this.config.baseUrl}/api/team/timezones`);
  }

  updatePresence(userId: string, request: UpdatePresenceRequest): Observable<TeamMemberDto> {
    return this.http.put<TeamMemberDto>(`${this.config.baseUrl}/api/team/members/${userId}/presence`, request);
  }

  inviteMember(request: InviteMemberRequest): Observable<TeamInvitationDto> {
    return this.http.post<TeamInvitationDto>(`${this.config.baseUrl}/api/team/invite`, request);
  }
}
