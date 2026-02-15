import { Component, computed, inject, OnInit, signal } from '@angular/core';
import {
  SectionHeader,
  Button,
  WeekSelector,
  ScheduleCard,
  type DayItem,
  type ScheduleTimezone,
} from 'components';
import { MeetingService, MeetingDto } from 'api';
import { AuthStore } from '../services/auth-store';

const DAY_LETTERS = ['M', 'T', 'W', 'T', 'F', 'S', 'S'];

@Component({
  selector: 'app-meetings',
  imports: [SectionHeader, Button, WeekSelector, ScheduleCard],
  template: `
    <div class="meetings-page">
      <lib-section-header title="Meetings">
        <lib-button variant="primary" size="sm">+ New</lib-button>
      </lib-section-header>

      <lib-week-selector
        [monthLabel]="monthLabel()"
        [days]="weekDays()"
        [(selectedDay)]="selectedDay"
      />

      <lib-section-header title="Today's Schedule" />

      <div class="schedule-list">
        @for (m of todayMeetings(); track m.id; let i = $index) {
          <lib-schedule-card
            [name]="m.title"
            [time]="formatTimeRange(m)"
            [dotColor]="m.accentColor || '#6366F1'"
            [badgeText]="m.attendees.length + ' attendees'"
            [timezones]="getTimezones(m)"
          />
        }
        @empty {
          <p class="empty-text">No meetings scheduled for today</p>
        }
      </div>
    </div>
  `,
  styles: `
    :host { display: block; }
    .meetings-page {
      display: flex;
      flex-direction: column;
      gap: 20px;
      padding: 28px 32px;
      max-width: 720px;
    }
    .schedule-list {
      display: flex;
      flex-direction: column;
      gap: 12px;
    }
    .empty-text {
      font-size: 13px;
      color: var(--ts-text-tertiary, #9ca3af);
    }
    @media (max-width: 767px) {
      .meetings-page { padding: 20px 16px; }
    }
  `,
})
export class MeetingsPage implements OnInit {
  private readonly meetingService = inject(MeetingService);
  private readonly auth = inject(AuthStore);

  protected readonly todayMeetings = signal<MeetingDto[]>([]);
  protected readonly selectedDay = signal<number | null>(null);

  protected readonly monthLabel = computed(() => {
    const now = new Date();
    return now.toLocaleDateString(undefined, { month: 'long', year: 'numeric' });
  });

  protected readonly weekDays = computed<DayItem[]>(() => {
    const today = new Date();
    const dow = today.getDay();
    const monday = new Date(today);
    monday.setDate(today.getDate() - ((dow + 6) % 7));

    return Array.from({ length: 7 }, (_, i) => {
      const d = new Date(monday);
      d.setDate(monday.getDate() + i);
      const num = d.getDate();
      return {
        letter: DAY_LETTERS[i],
        number: num,
        variant: num === today.getDate() ? 'selected' as const : 'default' as const,
      };
    });
  });

  ngOnInit() {
    this.meetingService
      .getTodaySchedule(this.auth.userId(), this.auth.timeZoneId())
      .subscribe((res) => {
        this.todayMeetings.set(res.meetings);
        this.selectedDay.set(new Date().getDate());
      });
  }

  formatTimeRange(m: MeetingDto): string {
    const fmt = (iso: string) =>
      new Date(iso).toLocaleTimeString([], { hour: 'numeric', minute: '2-digit' });
    return `${fmt(m.startTimeUtc)} - ${fmt(m.endTimeUtc)}`;
  }

  getTimezones(m: MeetingDto): ScheduleTimezone[] {
    const seen = new Set<string>();
    return m.attendees
      .filter((a) => {
        if (seen.has(a.timeZoneId)) return false;
        seen.add(a.timeZoneId);
        return true;
      })
      .map((a) => ({
        label: `${a.displayName.split(' ')[0]} (${a.timeZoneId.split('/').pop()?.replace(/_/g, ' ')})`,
      }));
  }
}
