import { Component, input } from '@angular/core';

@Component({
  selector: 'lib-badge',
  template: `
    <span class="badge" [style.background]="bgColor()" [style.color]="textColor()">
      <ng-content></ng-content>
    </span>
  `,
  styles: `
    :host {
      display: inline-flex;
    }
    .badge {
      display: inline-flex;
      align-items: center;
      padding: 5px 10px;
      border-radius: 12px;
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 11px;
      font-weight: 600;
      line-height: 1;
      white-space: nowrap;
    }
  `,
})
export class Badge {
  bgColor = input('var(--ts-badge-indigo-bg, #EEF2FF)');
  textColor = input('var(--ts-accent-primary, #6366F1)');
}
