import { Component, input, model, output } from '@angular/core';
import { Icon } from './icon';
import { Avatar } from './avatar';

export interface TopNavItem {
  icon: string;
  label: string;
  id: string;
}

@Component({
  selector: 'lib-top-nav',
  imports: [Icon, Avatar],
  template: `
    <nav class="top-nav">
      <div class="left">
        <div class="logo-icon">
          <span class="logo-letter">T</span>
        </div>
        <span class="logo-label">TeamSync</span>
      </div>
      <div class="center">
        @for (item of items(); track item.id) {
          <button
            class="nav-pill"
            [class.active]="activeItem() === item.id"
            (click)="activeItem.set(item.id)"
          >
            <lib-icon [name]="item.icon" [size]="16" />
            <span class="pill-label">{{ item.label }}</span>
          </button>
        }
      </div>
      <div class="right">
        <button class="bell-btn" (click)="bellClicked.emit()">
          <lib-icon name="bell" [size]="18" />
        </button>
        <lib-avatar
          [initials]="userInitials()"
          [size]="38"
          bgColor="var(--ts-accent-primary, #6366F1)"
          textColor="#FFFFFF"
          [fontSize]="14"
        />
      </div>
    </nav>
  `,
  styles: `
    :host {
      display: block;
    }
    .top-nav {
      display: flex;
      align-items: center;
      justify-content: space-between;
      height: 60px;
      padding: 0 32px;
      background: #ffffff;
      border-bottom: 1px solid var(--ts-border-subtle, #f3f4f6);
    }
    .left {
      display: flex;
      align-items: center;
      gap: 10px;
    }
    .logo-icon {
      display: flex;
      align-items: center;
      justify-content: center;
      width: 28px;
      height: 28px;
      border-radius: 8px;
      background: var(--ts-accent-primary, #6366f1);
    }
    .logo-letter {
      font-family: var(--ts-font-heading, 'Bricolage Grotesque', sans-serif);
      font-size: 14px;
      font-weight: 700;
      color: #ffffff;
    }
    .logo-label {
      font-family: var(--ts-font-heading, 'Bricolage Grotesque', sans-serif);
      font-size: 18px;
      font-weight: 700;
      color: var(--ts-text-primary, #1a1a1a);
    }
    .center {
      display: flex;
      align-items: center;
      gap: 4px;
    }
    .nav-pill {
      display: flex;
      align-items: center;
      gap: 6px;
      padding: 8px 14px;
      border: none;
      border-radius: 20px;
      background: transparent;
      cursor: pointer;
      color: var(--ts-text-secondary, #6b7280);
      transition: all 0.15s;
    }
    .pill-label {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 13px;
      font-weight: 500;
    }
    .nav-pill.active {
      background: var(--ts-accent-primary, #6366f1);
      color: #ffffff;
    }
    .nav-pill.active .pill-label {
      font-weight: 600;
    }
    .nav-pill:not(.active):hover {
      background: var(--ts-bg-card, #f6f7f8);
    }
    .right {
      display: flex;
      align-items: center;
      gap: 12px;
    }
    .bell-btn {
      display: flex;
      align-items: center;
      justify-content: center;
      width: 38px;
      height: 38px;
      border-radius: 19px;
      background: var(--ts-bg-card, #f6f7f8);
      border: none;
      cursor: pointer;
      color: var(--ts-text-primary, #1a1a1a);
      transition: opacity 0.15s;
    }
    .bell-btn:hover {
      opacity: 0.85;
    }
  `,
})
export class TopNav {
  items = input.required<TopNavItem[]>();
  activeItem = model('');
  userInitials = input('S');
  bellClicked = output();
}
