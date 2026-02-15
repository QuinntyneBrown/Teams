import { Component, inject, OnInit, signal } from '@angular/core';
import {
  SearchBar,
  SectionHeader,
  Badge,
  TimezoneCard,
  MeetingCard,
  ActivityItem,
} from 'components';
import { DashboardService, DashboardResult, MeetingDto, ActivityFeedItemDto, TimeZoneGroupCardDto } from 'api';
import { AuthStore } from '../services/auth-store';
import { DatePipe } from '@angular/common';

const TZ_FLAGS: Record<string, string> = {
  'America/New_York': '\u{1F1FA}\u{1F1F8}',
  'America/Chicago': '\u{1F1FA}\u{1F1F8}',
  'America/Los_Angeles': '\u{1F1FA}\u{1F1F8}',
  'Europe/London': '\u{1F1EC}\u{1F1E7}',
  'Europe/Berlin': '\u{1F1E9}\u{1F1EA}',
  'Europe/Paris': '\u{1F1EB}\u{1F1F7}',
  'Asia/Tokyo': '\u{1F1EF}\u{1F1F5}',
  'Asia/Kolkata': '\u{1F1EE}\u{1F1F3}',
  'Asia/Shanghai': '\u{1F1E8}\u{1F1F3}',
  'Australia/Sydney': '\u{1F1E6}\u{1F1FA}',
  'Pacific/Auckland': '\u{1F1F3}\u{1F1FF}',
};

function tzFlag(tzId: string): string {
  return TZ_FLAGS[tzId] ?? '\u{1F30D}';
}

function tzCity(displayLabel: string): string {
  return displayLabel.replace(/_/g, ' ').split('/').pop() ?? displayLabel;
}

const ACCENT_COLORS = ['#6366F1', '#FF6B6B', '#22C55E', '#FCD34D'];

@Component({
  selector: 'app-home',
  imports: [SearchBar, SectionHeader, Badge, TimezoneCard, MeetingCard, ActivityItem, DatePipe],
  template: `
    <div class="home-page">
      <lib-section-header title="Home">
        <lib-badge>{{ greeting() }}</lib-badge>
      </lib-section-header>

      <lib-search-bar placeholder="Search anything..." />

      <lib-section-header title="Team Time Zones">
        <lib-badge>{{ onlineCount() }} online</lib-badge>
      </lib-section-header>

      <div class="tz-row">
        @for (tz of timezones(); track tz.timeZoneId) {
          <lib-timezone-card
            [flag]="tzFlag(tz.timeZoneId)"
            [time]="tz.currentLocalTime"
            [city]="tzCity(tz.displayLabel)"
          />
        }
      </div>

      <div class="two-col">
        <div class="col">
          <lib-section-header title="Upcoming Meetings" />
          <div class="card-list">
            @for (m of meetings(); track m.id; let i = $index) {
              <lib-meeting-card
                [timeRange]="formatTimeRange(m)"
                [name]="m.title"
                [meta]="m.attendees.length + ' attendees'"
                [accentColor]="accentColors[i % accentColors.length]"
              />
            }
            @empty {
              <p class="empty-text">No upcoming meetings</p>
            }
          </div>
        </div>
        <div class="col">
          <lib-section-header title="Recent Activity" />
          <div class="card-list">
            @for (a of activities(); track a.id) {
              <lib-activity-item
                [initials]="a.actorInitials"
                [avatarBg]="a.actorAvatarColor + '22'"
                [avatarColor]="a.actorAvatarColor"
                [text]="a.actorDisplayName + ' ' + a.action"
                [meta]="(a.createdAt | date:'shortTime') ?? ''"
              />
            }
            @empty {
              <p class="empty-text">No recent activity</p>
            }
          </div>
        </div>
      </div>
    </div>
  `,
  styles: `
    :host { display: block; }
    .home-page {
      display: flex;
      flex-direction: column;
      gap: 20px;
      padding: 28px 32px;
      max-width: 960px;
    }
    .tz-row {
      display: flex;
      gap: 12px;
      overflow-x: auto;
    }
    .two-col {
      display: flex;
      gap: 24px;
    }
    .col {
      flex: 1;
      display: flex;
      flex-direction: column;
      gap: 12px;
      min-width: 0;
    }
    .card-list {
      display: flex;
      flex-direction: column;
      gap: 10px;
    }
    .empty-text {
      font-size: 13px;
      color: var(--ts-text-tertiary, #9ca3af);
    }
    @media (max-width: 767px) {
      .home-page { padding: 20px 16px; }
      .two-col { flex-direction: column; }
    }
  `,
})
export class HomePage implements OnInit {
  private readonly dashboard = inject(DashboardService);
  private readonly auth = inject(AuthStore);

  protected readonly timezones = signal<TimeZoneGroupCardDto[]>([]);
  protected readonly meetings = signal<MeetingDto[]>([]);
  protected readonly activities = signal<ActivityFeedItemDto[]>([]);
  protected readonly onlineCount = signal(0);
  protected readonly accentColors = ACCENT_COLORS;

  protected readonly greeting = signal('Dashboard');

  protected tzFlag = tzFlag;
  protected tzCity = tzCity;

  ngOnInit() {
    this.dashboard
      .getDashboard(this.auth.userId(), this.auth.timeZoneId())
      .subscribe((res) => {
        this.timezones.set(res.teamTimeZones);
        this.meetings.set(res.upcomingMeetings);
        this.activities.set(res.recentActivity);
        this.onlineCount.set(res.onlineTeamMemberCount);
      });
  }

  formatTimeRange(m: MeetingDto): string {
    const fmt = (iso: string) =>
      new Date(iso).toLocaleTimeString([], { hour: 'numeric', minute: '2-digit' });
    return `${fmt(m.startTimeUtc)} - ${fmt(m.endTimeUtc)}`;
  }
}
