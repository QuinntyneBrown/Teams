import { Component, input } from '@angular/core';
import { Avatar } from './avatar';

@Component({
  selector: 'lib-team-member-grid-card',
  imports: [Avatar],
  template: `
    <div class="member-card">
      <lib-avatar
        [initials]="initials()"
        [size]="56"
        [bgColor]="avatarBg()"
        [textColor]="avatarColor()"
        [fontSize]="22"
      />
      <div class="name-row">
        <span class="name">{{ name() }}</span>
        @if (flag()) {
          <span class="flag">{{ flag() }}</span>
        }
      </div>
      <span class="role">{{ role() }}</span>
      <span class="location">{{ location() }}</span>
      <div class="status-row">
        <span
          class="status-dot"
          [style.background]="status() === 'online' ? 'var(--ts-accent-green, #22C55E)' : 'var(--ts-accent-yellow, #FCD34D)'"
        ></span>
        <span
          class="status-text"
          [style.color]="status() === 'online' ? 'var(--ts-accent-green, #22C55E)' : 'var(--ts-accent-yellow, #FCD34D)'"
        >
          {{ status() === 'online' ? 'Online' : 'Away' }}
        </span>
      </div>
      <span class="time">{{ time() }}</span>
    </div>
  `,
  styles: `
    :host {
      display: block;
      flex: 1;
      min-width: 0;
    }
    .member-card {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 16px;
      padding: 20px;
      border-radius: 16px;
      background: #ffffff;
    }
    .name-row {
      display: flex;
      align-items: center;
      gap: 6px;
    }
    .name {
      font-family: var(--ts-font-body, 'Inter', sans-serif);
      font-size: 16px;
      font-weight: 600;
      color: var(--ts-text-primary, #1a1a1a);
    }
    .flag {
      font-size: 16px;
    }
    .role {
      font-family: var(--ts-font-body, 'Inter', sans-serif);
      font-size: 13px;
      color: var(--ts-text-secondary, #6b7280);
      text-align: center;
    }
    .location {
      font-family: var(--ts-font-body, 'Inter', sans-serif);
      font-size: 12px;
      color: var(--ts-text-tertiary, #9ca3af);
      text-align: center;
    }
    .status-row {
      display: flex;
      align-items: center;
      gap: 6px;
    }
    .status-dot {
      width: 8px;
      height: 8px;
      border-radius: 50%;
    }
    .status-text {
      font-family: var(--ts-font-body, 'Inter', sans-serif);
      font-size: 12px;
    }
    .time {
      font-family: var(--ts-font-body, 'Inter', sans-serif);
      font-size: 12px;
      color: var(--ts-text-tertiary, #9ca3af);
      text-align: center;
    }
  `,
})
export class TeamMemberGridCard {
  initials = input.required<string>();
  avatarBg = input('#E0E7FF');
  avatarColor = input('#6366F1');
  name = input.required<string>();
  flag = input('');
  role = input.required<string>();
  location = input.required<string>();
  status = input<'online' | 'away'>('online');
  time = input('');
}
