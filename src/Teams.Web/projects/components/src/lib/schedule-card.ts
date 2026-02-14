import { Component, input, output } from '@angular/core';
import { Badge } from './badge';

export interface ScheduleTimezone {
  label: string;
}

@Component({
  selector: 'lib-schedule-card',
  imports: [Badge],
  template: `
    <div class="schedule-card">
      <div class="top">
        <div class="left">
          <span class="dot" [style.background]="dotColor()"></span>
          <span class="name">{{ name() }}</span>
        </div>
        @if (badgeText()) {
          <lib-badge [bgColor]="badgeBg()" [textColor]="badgeColor()">
            {{ badgeText() }}
          </lib-badge>
        }
      </div>
      <span class="time">{{ time() }}</span>
      @if (timezones().length > 0) {
        <div class="tz-section">
          <span class="tz-label">For your team:</span>
          <div class="tz-row">
            @for (tz of timezones(); track tz.label) {
              <span class="tz-item">{{ tz.label }}</span>
            }
          </div>
        </div>
      }
      <ng-content></ng-content>
    </div>
  `,
  styles: `
    :host {
      display: block;
    }
    .schedule-card {
      display: flex;
      flex-direction: column;
      gap: 12px;
      padding: 16px;
      border-radius: 16px;
      background: var(--ts-bg-card, #f6f7f8);
    }
    .top {
      display: flex;
      align-items: center;
      justify-content: space-between;
      width: 100%;
    }
    .left {
      display: flex;
      align-items: center;
      gap: 10px;
    }
    .dot {
      width: 10px;
      height: 10px;
      border-radius: 5px;
      flex-shrink: 0;
    }
    .name {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 15px;
      font-weight: 600;
      color: var(--ts-text-primary, #1a1a1a);
    }
    .time {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 13px;
      color: var(--ts-text-secondary, #6b7280);
    }
    .tz-section {
      display: flex;
      flex-direction: column;
      gap: 4px;
      padding-top: 10px;
      border-top: 1px solid var(--ts-border-subtle, #f3f4f6);
      width: 100%;
    }
    .tz-label {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 11px;
      font-weight: 500;
      color: var(--ts-text-tertiary, #9ca3af);
    }
    .tz-row {
      display: flex;
      gap: 12px;
      width: 100%;
    }
    .tz-item {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 12px;
      font-weight: 500;
      color: var(--ts-text-secondary, #6b7280);
    }
  `,
})
export class ScheduleCard {
  dotColor = input('var(--ts-accent-primary, #6366F1)');
  name = input.required<string>();
  time = input.required<string>();
  badgeText = input('');
  badgeBg = input('var(--ts-badge-indigo-bg, #EEF2FF)');
  badgeColor = input('var(--ts-accent-primary, #6366F1)');
  timezones = input<ScheduleTimezone[]>([]);
}
