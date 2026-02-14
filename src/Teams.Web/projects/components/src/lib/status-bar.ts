import { Component, input } from '@angular/core';
import { Icon } from './icon';

@Component({
  selector: 'lib-status-bar',
  imports: [Icon],
  template: `
    <div class="status-bar">
      <span class="time">{{ time() }}</span>
      <div class="icons">
        <lib-icon name="signal" [size]="16" />
        <lib-icon name="wifi" [size]="16" />
        <lib-icon name="battery-full" [size]="16" />
      </div>
    </div>
  `,
  styles: `
    :host {
      display: block;
    }
    .status-bar {
      display: flex;
      align-items: center;
      justify-content: space-between;
      height: 54px;
      padding: 14px 24px 0 24px;
      color: var(--ts-text-primary, #1a1a1a);
    }
    .time {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 15px;
      font-weight: 600;
    }
    .icons {
      display: flex;
      align-items: center;
      gap: 6px;
    }
  `,
})
export class StatusBar {
  time = input('9:41');
}
