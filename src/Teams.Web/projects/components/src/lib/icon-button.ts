import { Component, input, output } from '@angular/core';

@Component({
  selector: 'lib-icon-button',
  template: `
    <button
      class="icon-btn"
      [style.width.px]="size()"
      [style.height.px]="size()"
      [style.border-radius.px]="size() / 2"
      [style.background]="bgColor()"
      (click)="clicked.emit()"
    >
      <ng-content></ng-content>
    </button>
  `,
  styles: `
    :host {
      display: inline-flex;
    }
    .icon-btn {
      display: flex;
      align-items: center;
      justify-content: center;
      border: none;
      cursor: pointer;
      padding: 0;
      transition: opacity 0.15s;
      flex-shrink: 0;
    }
    .icon-btn:hover {
      opacity: 0.85;
    }
  `,
})
export class IconButton {
  size = input(42);
  bgColor = input('var(--ts-bg-card, #F6F7F8)');
  clicked = output();
}
