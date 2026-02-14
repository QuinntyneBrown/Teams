import { Component, input, model } from '@angular/core';
import { Icon } from './icon';

export interface TabItem {
  icon: string;
  label: string;
  id: string;
}

@Component({
  selector: 'lib-tab-bar',
  imports: [Icon],
  template: `
    <nav class="tab-bar">
      @for (tab of tabs(); track tab.id) {
        <button
          class="tab"
          [class.active]="activeTab() === tab.id"
          (click)="activeTab.set(tab.id)"
        >
          <lib-icon [name]="tab.icon" [size]="22" />
          <span class="label">{{ tab.label }}</span>
        </button>
      }
    </nav>
  `,
  styles: `
    :host {
      display: block;
    }
    .tab-bar {
      display: flex;
      align-items: center;
      justify-content: space-between;
      height: 82px;
      padding: 12px 32px 28px 32px;
      background: #ffffff;
      border-top: 1px solid var(--ts-border-subtle, #f3f4f6);
    }
    .tab {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 4px;
      flex: 1;
      background: none;
      border: none;
      cursor: pointer;
      padding: 0;
      color: var(--ts-text-disabled, #d1d5db);
      transition: color 0.15s;
    }
    .tab.active {
      color: var(--ts-accent-primary, #6366f1);
    }
    .label {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 10px;
      font-weight: 500;
    }
    .tab.active .label {
      font-weight: 600;
    }
  `,
})
export class TabBar {
  tabs = input.required<TabItem[]>();
  activeTab = model('');
}
