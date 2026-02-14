import { Component, input } from '@angular/core';

@Component({
  selector: 'lib-timezone-card',
  template: `
    <div class="tz-card">
      <span class="flag">{{ flag() }}</span>
      <span class="time">{{ time() }}</span>
      <span class="city">{{ city() }}</span>
    </div>
  `,
  styles: `
    :host {
      display: block;
      flex: 1;
      min-width: 0;
    }
    .tz-card {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 8px;
      padding: 14px;
      border-radius: 16px;
      background: var(--ts-bg-card, #f6f7f8);
    }
    .flag {
      font-size: 24px;
      line-height: 1;
    }
    .time {
      font-family: var(--ts-font-heading, 'Bricolage Grotesque', sans-serif);
      font-size: 16px;
      font-weight: 700;
      color: var(--ts-text-primary, #1a1a1a);
    }
    .city {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 11px;
      color: var(--ts-text-tertiary, #9ca3af);
    }
  `,
})
export class TimezoneCard {
  flag = input.required<string>();
  time = input.required<string>();
  city = input.required<string>();
}
