import { Component, input } from '@angular/core';
import { Avatar } from './avatar';

@Component({
  selector: 'lib-activity-item',
  imports: [Avatar],
  template: `
    <div class="activity-item">
      <lib-avatar
        [initials]="initials()"
        [size]="36"
        [bgColor]="avatarBg()"
        [textColor]="avatarColor()"
        [fontSize]="14"
      />
      <div class="info">
        <span class="text">{{ text() }}</span>
        <span class="meta">{{ meta() }}</span>
      </div>
    </div>
  `,
  styles: `
    :host {
      display: block;
    }
    .activity-item {
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
    .text {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 14px;
      font-weight: 500;
      color: var(--ts-text-primary, #1a1a1a);
    }
    .meta {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 12px;
      color: var(--ts-text-tertiary, #9ca3af);
    }
  `,
})
export class ActivityItem {
  initials = input.required<string>();
  avatarBg = input('#E0E7FF');
  avatarColor = input('#6366F1');
  text = input.required<string>();
  meta = input.required<string>();
}
