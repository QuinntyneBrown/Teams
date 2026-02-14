import { Component, input, output } from '@angular/core';

@Component({
  selector: 'lib-channel-list-item',
  template: `
    <button class="channel-item" [class.active]="active()" (click)="clicked.emit()">
      <span class="hash">#</span>
      <span class="name">{{ name() }}</span>
    </button>
  `,
  styles: `
    :host {
      display: block;
    }
    .channel-item {
      display: flex;
      align-items: center;
      gap: 8px;
      width: 100%;
      height: 40px;
      padding: 0 10px;
      border: none;
      border-radius: 10px;
      background: transparent;
      cursor: pointer;
      transition: background 0.15s;
    }
    .hash {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 16px;
      font-weight: 500;
      color: var(--ts-text-tertiary, #9ca3af);
    }
    .name {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 13px;
      font-weight: 500;
      color: var(--ts-text-secondary, #6b7280);
    }
    .channel-item.active {
      background: var(--ts-accent-primary, #6366f1);
    }
    .channel-item.active .hash,
    .channel-item.active .name {
      color: #ffffff;
      font-weight: 600;
    }
    .channel-item:not(.active):hover {
      background: var(--ts-bg-card, #f6f7f8);
    }
  `,
})
export class ChannelListItem {
  name = input.required<string>();
  active = input(false);
  clicked = output();
}
