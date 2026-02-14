import { Component, computed, input } from '@angular/core';

@Component({
  selector: 'lib-avatar',
  template: `
    <div
      class="avatar"
      [style.width.px]="size()"
      [style.height.px]="size()"
      [style.border-radius.px]="size() / 2"
      [style.background]="bgColor()"
      [style.font-size.px]="computedFontSize()"
    >
      <span [style.color]="textColor()">{{ initials() }}</span>
    </div>
  `,
  styles: `
    :host {
      display: inline-flex;
    }
    .avatar {
      display: flex;
      align-items: center;
      justify-content: center;
      flex-shrink: 0;
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-weight: 700;
    }
  `,
})
export class Avatar {
  initials = input.required<string>();
  size = input(42);
  bgColor = input('#E0E7FF');
  textColor = input('#6366F1');
  fontSize = input<number | undefined>(undefined);

  computedFontSize = computed(() => this.fontSize() ?? Math.round(this.size() * 0.38));
}
