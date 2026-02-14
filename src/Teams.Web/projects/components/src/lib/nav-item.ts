import { Component, input, output } from '@angular/core';
import { Icon } from './icon';

@Component({
  selector: 'lib-nav-item',
  imports: [Icon],
  template: `
    <button class="nav-item" [class.active]="active()" (click)="clicked.emit()">
      <lib-icon [name]="icon()" [size]="18" />
      <span class="label">{{ label() }}</span>
    </button>
  `,
  styles: `
    :host {
      display: block;
    }
    .nav-item {
      display: flex;
      align-items: center;
      gap: 12px;
      width: 100%;
      height: 44px;
      padding: 0 12px;
      border: none;
      border-radius: 12px;
      background: transparent;
      cursor: pointer;
      color: var(--ts-text-secondary, #6b7280);
      transition: all 0.15s;
    }
    .label {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 14px;
      font-weight: 500;
    }
    .nav-item.active {
      background: var(--ts-accent-primary, #6366f1);
      color: #ffffff;
    }
    .nav-item.active .label {
      font-weight: 600;
    }
    .nav-item:not(.active):hover {
      background: var(--ts-bg-card, #f6f7f8);
    }
  `,
})
export class NavItem {
  icon = input.required<string>();
  label = input.required<string>();
  active = input(false);
  clicked = output();
}
