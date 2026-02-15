import { inject, Injectable, signal } from '@angular/core';
import { AuthService } from 'api';

@Injectable({ providedIn: 'root' })
export class AuthStore {
  private readonly authService = inject(AuthService);

  readonly userId = signal('');
  readonly displayName = signal('');
  readonly initials = signal('');
  readonly timeZoneId = signal(Intl.DateTimeFormat().resolvedOptions().timeZone);

  autoLogin() {
    this.authService.loginAs('sarah@teamsync.com').subscribe((res) => {
      localStorage.setItem('auth_token', res.token);
      this.userId.set(res.userId);
      this.displayName.set(res.displayName);
      this.initials.set(
        res.displayName
          .split(' ')
          .map((p) => p[0])
          .join('')
          .toUpperCase()
          .slice(0, 2)
      );
    });
  }
}
