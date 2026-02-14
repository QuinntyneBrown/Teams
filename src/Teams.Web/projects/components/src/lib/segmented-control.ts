import { Component, input, model } from '@angular/core';

export interface SegmentItem {
  label: string;
  value: string;
}

@Component({
  selector: 'lib-segmented-control',
  template: `
    <div class="segmented">
      @for (item of items(); track item.value) {
        <button
          class="segment"
          [class.active]="activeValue() === item.value"
          (click)="activeValue.set(item.value)"
        >
          {{ item.label }}
        </button>
      }
    </div>
  `,
  styles: `
    :host {
      display: block;
    }
    .segmented {
      display: flex;
      height: 44px;
      padding: 4px;
      border-radius: 22px;
      background: var(--ts-bg-card, #f6f7f8);
    }
    .segment {
      flex: 1;
      display: flex;
      align-items: center;
      justify-content: center;
      border: none;
      border-radius: 18px;
      background: transparent;
      cursor: pointer;
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 13px;
      font-weight: 500;
      color: var(--ts-text-tertiary, #9ca3af);
      transition: all 0.15s;
    }
    .segment.active {
      background: #ffffff;
      color: var(--ts-text-primary, #1a1a1a);
      font-weight: 600;
    }
  `,
})
export class SegmentedControl {
  items = input.required<SegmentItem[]>();
  activeValue = model('');
}
