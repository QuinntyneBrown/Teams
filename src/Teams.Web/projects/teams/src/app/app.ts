import { Component, computed, effect, inject, OnInit, signal } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { filter } from 'rxjs';
import {
  SidebarNav,
  TopNav,
  TabBar,
  StatusBar,
  type SidebarNavItem,
  type SidebarUser,
  type TabItem,
  type TopNavItem,
} from 'components';
import { AuthStore } from './services/auth-store';

const NAV_ITEMS: SidebarNavItem[] & TopNavItem[] & TabItem[] = [
  { icon: 'house', label: 'Home', id: 'home' },
  { icon: 'message-circle', label: 'Chat', id: 'chat' },
  { icon: 'calendar', label: 'Meetings', id: 'meetings' },
  { icon: 'users', label: 'Team', id: 'team' },
];

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, SidebarNav, TopNav, TabBar, StatusBar],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App implements OnInit {
  private readonly router = inject(Router);
  private readonly auth = inject(AuthStore);

  protected readonly navItems = NAV_ITEMS;
  protected readonly activeItem = signal('home');

  protected readonly sidebarUser = computed<SidebarUser | null>(() => {
    const name = this.auth.displayName();
    if (!name) return null;
    return { initials: this.auth.initials(), name, role: 'Product Manager' };
  });

  protected readonly userInitials = computed(() => this.auth.initials() || 'S');

  constructor() {
    this.router.events
      .pipe(filter((e): e is NavigationEnd => e instanceof NavigationEnd))
      .subscribe((e) => {
        const segment = e.urlAfterRedirects.split('/')[1] || 'home';
        this.activeItem.set(segment);
      });

    effect(() => {
      const id = this.activeItem();
      const currentSegment = this.router.url.split('/')[1] || 'home';
      if (id && id !== currentSegment) {
        this.router.navigate(['/' + id]);
      }
    });
  }

  ngOnInit() {
    this.auth.autoLogin();
  }
}
