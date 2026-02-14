import { Component, input, model, output } from '@angular/core';
import { Icon } from './icon';

export interface DayItem {
  letter: string;
  number: number;
  variant?: 'default' | 'selected' | 'highlighted';
}

@Component({
  selector: 'lib-week-selector',
  imports: [Icon],
  template: `
    <div class="week-selector">
      <div class="header">
        <span class="label">{{ monthLabel() }}</span>
        <div class="nav">
          <button class="nav-btn" (click)="navigatePrev.emit()">
            <lib-icon name="chevron-left" [size]="18" />
          </button>
          <button class="nav-btn" (click)="navigateNext.emit()">
            <lib-icon name="chevron-right" [size]="18" />
          </button>
        </div>
      </div>
      <div class="days">
        @for (day of days(); track day.number) {
          <div class="day" (click)="selectedDay.set(day.number)">
            <span class="day-letter">{{ day.letter }}</span>
            <div
              class="day-circle"
              [class.selected]="day.variant === 'selected' || selectedDay() === day.number"
              [class.highlighted]="day.variant === 'highlighted'"
              [class.default]="day.variant !== 'selected' && day.variant !== 'highlighted' && selectedDay() !== day.number"
            >
              <span class="day-number">{{ day.number }}</span>
            </div>
          </div>
        }
      </div>
    </div>
  `,
  styles: `
    :host {
      display: block;
    }
    .week-selector {
      display: flex;
      flex-direction: column;
      gap: 14px;
      padding: 16px;
      border-radius: 20px;
      background: var(--ts-bg-card, #f6f7f8);
    }
    .header {
      display: flex;
      align-items: center;
      justify-content: space-between;
      width: 100%;
    }
    .label {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 14px;
      font-weight: 600;
      color: var(--ts-text-primary, #1a1a1a);
    }
    .nav {
      display: flex;
      align-items: center;
      gap: 12px;
    }
    .nav-btn {
      display: flex;
      align-items: center;
      justify-content: center;
      background: none;
      border: none;
      cursor: pointer;
      padding: 0;
      color: var(--ts-text-tertiary, #9ca3af);
    }
    .days {
      display: flex;
      justify-content: space-between;
      width: 100%;
    }
    .day {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 6px;
      cursor: pointer;
    }
    .day-letter {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 11px;
      font-weight: 600;
      color: var(--ts-text-tertiary, #9ca3af);
    }
    .day-circle {
      display: flex;
      align-items: center;
      justify-content: center;
      width: 36px;
      height: 36px;
      border-radius: 18px;
    }
    .day-circle.default {
      background: var(--ts-bg-card, #f6f7f8);
      border: 1.5px solid var(--ts-border-empty, #e5e7eb);
    }
    .day-circle.default .day-number {
      color: var(--ts-text-secondary, #6b7280);
      font-weight: 600;
    }
    .day-circle.selected {
      background: var(--ts-accent-primary, #6366f1);
      border: none;
    }
    .day-circle.selected .day-number {
      color: #ffffff;
      font-weight: 700;
    }
    .day-circle.highlighted {
      background: var(--ts-accent-coral, #ff6b6b);
      border: none;
    }
    .day-circle.highlighted .day-number {
      color: #ffffff;
      font-weight: 700;
    }
    .day-number {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 14px;
    }
  `,
})
export class WeekSelector {
  monthLabel = input.required<string>();
  days = input.required<DayItem[]>();
  selectedDay = model<number | null>(null);
  navigatePrev = output();
  navigateNext = output();
}
