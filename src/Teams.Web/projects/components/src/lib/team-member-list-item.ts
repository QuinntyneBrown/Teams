import { Component, input } from '@angular/core';
import { Avatar } from './avatar';

@Component({
  selector: 'lib-team-member-list-item',
  imports: [Avatar],
  template: `
    <div class="member-item">
      <lib-avatar
        [initials]="initials()"
        [size]="44"
        [bgColor]="avatarBg()"
        [textColor]="avatarColor()"
        [fontSize]="16"
      />
      <div class="info">
        <div class="top-row">
          <div class="name-group">
            <span class="name">{{ name() }}</span>
            @if (flag()) {
              <span class="flag">{{ flag() }}</span>
            }
          </div>
          @if (timeBadge()) {
            <span
              class="time-badge"
              [style.background]="timeBadgeBg()"
              [style.color]="timeBadgeColor()"
              [style.border]="isOnline() ? 'none' : '1px solid var(--ts-border-empty, #E5E7EB)'"
            >
              {{ timeBadge() }}
            </span>
          }
        </div>
        <span class="role">{{ role() }}</span>
      </div>
    </div>
  `,
  styles: `
    :host {
      display: block;
    }
    .member-item {
      display: flex;
      align-items: center;
      gap: 12px;
      padding: 14px;
      border-radius: 16px;
      background: var(--ts-bg-card, #f6f7f8);
    }
    .info {
      display: flex;
      flex-direction: column;
      gap: 2px;
      min-width: 0;
      flex: 1;
    }
    .top-row {
      display: flex;
      align-items: center;
      justify-content: space-between;
      width: 100%;
    }
    .name-group {
      display: flex;
      align-items: center;
      gap: 6px;
    }
    .name {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 15px;
      font-weight: 600;
      color: var(--ts-text-primary, #1a1a1a);
    }
    .flag {
      font-size: 14px;
    }
    .time-badge {
      display: inline-flex;
      align-items: center;
      padding: 3px 8px;
      border-radius: 8px;
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 11px;
      font-weight: 500;
    }
    .role {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 13px;
      color: var(--ts-text-secondary, #6b7280);
    }
  `,
})
export class TeamMemberListItem {
  initials = input.required<string>();
  avatarBg = input('#E0E7FF');
  avatarColor = input('#6366F1');
  name = input.required<string>();
  flag = input('');
  role = input.required<string>();
  timeBadge = input('');
  timeBadgeBg = input('var(--ts-badge-green-bg, #F0FDF4)');
  timeBadgeColor = input('var(--ts-accent-green, #22C55E)');
  isOnline = input(true);
}
