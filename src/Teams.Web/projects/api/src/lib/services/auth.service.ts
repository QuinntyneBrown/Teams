import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_CONFIG } from '../api-config';
import { LoginRequest, LoginResponse } from '../models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly config = inject(API_CONFIG);

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.config.baseUrl}/api/auth/login`, request);
  }

  loginAs(email: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.config.baseUrl}/api/auth/login-as/${encodeURIComponent(email)}`, null);
  }
}
