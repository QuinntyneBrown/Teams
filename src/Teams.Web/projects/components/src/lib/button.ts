import { Component, input, output } from '@angular/core';

@Component({
  selector: 'lib-button',
  template: `
    <button
      class="btn"
      [class.btn-primary]="variant() === 'primary'"
      [class.btn-secondary]="variant() === 'secondary'"
      [class.btn-sm]="size() === 'sm'"
      (click)="clicked.emit()"
    >
      <ng-content></ng-content>
    </button>
  `,
  styles: `
    :host {
      display: inline-flex;
    }
    .btn {
      display: inline-flex;
      align-items: center;
      gap: 6px;
      padding: 8px 14px;
      border-radius: 20px;
      border: none;
      cursor: pointer;
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 12px;
      font-weight: 600;
      line-height: 1;
      transition: opacity 0.15s;
    }
    .btn:hover {
      opacity: 0.9;
    }
    .btn-primary {
      background: var(--ts-accent-primary, #6366f1);
      color: #ffffff;
    }
    .btn-secondary {
      background: var(--ts-bg-card, #f6f7f8);
      color: var(--ts-text-primary, #1a1a1a);
    }
    .btn-sm {
      padding: 6px 12px;
      font-size: 11px;
    }
  `,
})
export class Button {
  variant = input<'primary' | 'secondary'>('primary');
  size = input<'sm' | 'md'>('md');
  clicked = output();
}
