import { Component, input, model } from '@angular/core';
import { NavItem } from './nav-item';
import { Avatar } from './avatar';

export interface SidebarNavItem {
  icon: string;
  label: string;
  id: string;
}

export interface SidebarUser {
  initials: string;
  name: string;
  role: string;
}

@Component({
  selector: 'lib-sidebar-nav',
  imports: [NavItem, Avatar],
  template: `
    <aside class="sidebar">
      <div class="top">
        <div class="logo">
          <div class="logo-icon">
            <span class="logo-letter">T</span>
          </div>
          <span class="logo-label">TeamSync</span>
        </div>
        <nav class="nav-items">
          @for (item of items(); track item.id) {
            <lib-nav-item
              [icon]="item.icon"
              [label]="item.label"
              [active]="activeItem() === item.id"
              (clicked)="activeItem.set(item.id)"
            />
          }
        </nav>
      </div>
      @if (user(); as u) {
        <div class="bottom">
          <lib-avatar
            [initials]="u.initials"
            [size]="36"
            bgColor="var(--ts-accent-primary, #6366F1)"
            textColor="#FFFFFF"
            [fontSize]="14"
          />
          <div class="user-info">
            <span class="user-name">{{ u.name }}</span>
            <span class="user-role">{{ u.role }}</span>
          </div>
        </div>
      }
    </aside>
  `,
  styles: `
    :host {
      display: block;
    }
    .sidebar {
      display: flex;
      flex-direction: column;
      justify-content: space-between;
      width: 240px;
      height: 100%;
      background: #ffffff;
      border-right: 1px solid var(--ts-border-subtle, #f3f4f6);
    }
    .top {
      display: flex;
      flex-direction: column;
      gap: 32px;
      padding: 28px 20px 0 20px;
    }
    .logo {
      display: flex;
      align-items: center;
      gap: 10px;
      padding: 0 4px;
    }
    .logo-icon {
      display: flex;
      align-items: center;
      justify-content: center;
      width: 32px;
      height: 32px;
      border-radius: 10px;
      background: var(--ts-accent-primary, #6366f1);
    }
    .logo-letter {
      font-family: var(--ts-font-heading, 'Bricolage Grotesque', sans-serif);
      font-size: 16px;
      font-weight: 700;
      color: #ffffff;
    }
    .logo-label {
      font-family: var(--ts-font-heading, 'Bricolage Grotesque', sans-serif);
      font-size: 20px;
      font-weight: 700;
      color: var(--ts-text-primary, #1a1a1a);
    }
    .nav-items {
      display: flex;
      flex-direction: column;
      gap: 4px;
    }
    .bottom {
      display: flex;
      align-items: center;
      gap: 12px;
      padding: 16px 20px 24px 20px;
      border-top: 1px solid var(--ts-border-subtle, #f3f4f6);
    }
    .user-info {
      display: flex;
      flex-direction: column;
      gap: 1px;
      min-width: 0;
      flex: 1;
    }
    .user-name {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 13px;
      font-weight: 600;
      color: var(--ts-text-primary, #1a1a1a);
    }
    .user-role {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 11px;
      color: var(--ts-text-tertiary, #9ca3af);
    }
  `,
})
export class SidebarNav {
  items = input.required<SidebarNavItem[]>();
  activeItem = model('');
  user = input<SidebarUser | null>(null);
}
