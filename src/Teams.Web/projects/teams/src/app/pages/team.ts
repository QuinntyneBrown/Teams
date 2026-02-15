import { Component, computed, inject, OnInit, signal } from '@angular/core';
import {
  SectionHeader,
  Button,
  SearchBar,
  SegmentedControl,
  TeamMemberListItem,
  TeamMemberGridCard,
  type SegmentItem,
} from 'components';
import { TeamService, TeamMemberDto } from 'api';
import { AuthStore } from '../services/auth-store';

@Component({
  selector: 'app-team',
  imports: [
    SectionHeader,
    Button,
    SearchBar,
    SegmentedControl,
    TeamMemberListItem,
    TeamMemberGridCard,
  ],
  template: `
    <div class="team-page">
      <lib-section-header title="Team">
        <lib-button variant="primary" size="sm">+ Invite</lib-button>
      </lib-section-header>

      <lib-search-bar placeholder="Search members..." [(value)]="searchQuery" />

      <lib-segmented-control
        [items]="segments()"
        [(activeValue)]="activeSegment"
      />

      <!-- Mobile list -->
      <div class="member-list mobile-only">
        @for (m of filteredMembers(); track m.id) {
          <lib-team-member-list-item
            [initials]="m.initials"
            [avatarBg]="m.avatarColor + '22'"
            [avatarColor]="m.avatarColor"
            [name]="m.displayName"
            [role]="m.role"
            [isOnline]="m.status === 'online'"
            [timeBadge]="getLocalTime(m)"
            [timeBadgeBg]="m.status === 'online' ? 'var(--ts-badge-green-bg, #F0FDF4)' : 'transparent'"
            [timeBadgeColor]="m.status === 'online' ? 'var(--ts-accent-green, #22C55E)' : 'var(--ts-text-tertiary, #9ca3af)'"
          />
        }
        @empty {
          <p class="empty-text">No members found</p>
        }
      </div>

      <!-- Desktop/Tablet grid -->
      <div class="member-grid desktop-only">
        @for (m of filteredMembers(); track m.id) {
          <lib-team-member-grid-card
            [initials]="m.initials"
            [avatarBg]="m.avatarColor + '22'"
            [avatarColor]="m.avatarColor"
            [name]="m.displayName"
            [role]="m.role"
            [location]="getLocation(m)"
            [status]="m.status === 'online' ? 'online' : 'away'"
            [time]="getLocalTime(m)"
          />
        }
        @empty {
          <p class="empty-text">No members found</p>
        }
      </div>
    </div>
  `,
  styles: `
    :host { display: block; }
    .team-page {
      display: flex;
      flex-direction: column;
      gap: 20px;
      padding: 28px 32px;
      max-width: 960px;
    }
    .member-list {
      display: flex;
      flex-direction: column;
      gap: 10px;
    }
    .member-grid {
      display: grid;
      grid-template-columns: repeat(3, 1fr);
      gap: 16px;
    }
    .empty-text {
      font-size: 13px;
      color: var(--ts-text-tertiary, #9ca3af);
    }
    .mobile-only { display: none; }
    .desktop-only { display: grid; }

    @media (min-width: 768px) and (max-width: 1023px) {
      .member-grid {
        grid-template-columns: repeat(2, 1fr);
      }
    }

    @media (max-width: 767px) {
      .team-page { padding: 20px 16px; }
      .mobile-only { display: flex; }
      .desktop-only { display: none; }
    }
  `,
})
export class TeamPage implements OnInit {
  private readonly teamService = inject(TeamService);
  private readonly auth = inject(AuthStore);

  private readonly members = signal<TeamMemberDto[]>([]);
  protected readonly searchQuery = signal('');
  protected readonly activeSegment = signal('all');

  protected readonly segments = computed<SegmentItem[]>(() => {
    const all = this.members();
    const online = all.filter((m) => m.status === 'online').length;
    const away = all.filter((m) => m.status !== 'online').length;
    return [
      { label: `All (${all.length})`, value: 'all' },
      { label: `Online (${online})`, value: 'online' },
      { label: `Away (${away})`, value: 'away' },
    ];
  });

  protected readonly filteredMembers = computed(() => {
    let list = this.members();
    const seg = this.activeSegment();
    if (seg === 'online') list = list.filter((m) => m.status === 'online');
    else if (seg === 'away') list = list.filter((m) => m.status !== 'online');

    const q = this.searchQuery().toLowerCase();
    if (q) {
      list = list.filter(
        (m) =>
          m.displayName.toLowerCase().includes(q) ||
          m.role.toLowerCase().includes(q)
      );
    }
    return list;
  });

  ngOnInit() {
    this.teamService.getMembers().subscribe((members) => {
      this.members.set(members);
    });
  }

  getLocalTime(m: TeamMemberDto): string {
    try {
      return new Date().toLocaleTimeString([], {
        hour: 'numeric',
        minute: '2-digit',
        timeZone: m.timeZoneId,
      });
    } catch {
      return '';
    }
  }

  getLocation(m: TeamMemberDto): string {
    return m.timeZoneId.split('/').pop()?.replace(/_/g, ' ') ?? m.timeZoneId;
  }
}
