import { Component, input } from '@angular/core';

@Component({
  selector: 'lib-meeting-card',
  template: `
    <div class="meeting-card">
      <div class="accent" [style.background]="accentColor()"></div>
      <div class="info">
        <span class="time" [style.color]="accentColor()">{{ timeRange() }}</span>
        <span class="name">{{ name() }}</span>
        <span class="meta">{{ meta() }}</span>
      </div>
    </div>
  `,
  styles: `
    :host {
      display: block;
    }
    .meeting-card {
      display: flex;
      align-items: center;
      gap: 14px;
      padding: 16px;
      border-radius: 16px;
      background: var(--ts-bg-card, #f6f7f8);
    }
    .accent {
      width: 4px;
      height: 48px;
      border-radius: 2px;
      flex-shrink: 0;
    }
    .info {
      display: flex;
      flex-direction: column;
      gap: 4px;
      min-width: 0;
      flex: 1;
    }
    .time {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 12px;
      font-weight: 500;
    }
    .name {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 15px;
      font-weight: 600;
      color: var(--ts-text-primary, #1a1a1a);
    }
    .meta {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 13px;
      color: var(--ts-text-secondary, #6b7280);
    }
  `,
})
export class MeetingCard {
  accentColor = input('var(--ts-accent-primary, #6366F1)');
  timeRange = input.required<string>();
  name = input.required<string>();
  meta = input.required<string>();
}
