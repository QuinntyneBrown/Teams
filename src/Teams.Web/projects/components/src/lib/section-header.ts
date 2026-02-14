import { Component, input } from '@angular/core';

@Component({
  selector: 'lib-section-header',
  template: `
    <div class="section-header">
      <h3 class="title">{{ title() }}</h3>
      <div class="action">
        <ng-content></ng-content>
      </div>
    </div>
  `,
  styles: `
    :host {
      display: block;
    }
    .section-header {
      display: flex;
      align-items: center;
      justify-content: space-between;
      width: 100%;
    }
    .title {
      margin: 0;
      font-family: var(--ts-font-heading, 'Bricolage Grotesque', sans-serif);
      font-size: 18px;
      font-weight: 700;
      color: var(--ts-text-primary, #1a1a1a);
    }
    .action {
      display: flex;
      align-items: center;
    }
  `,
})
export class SectionHeader {
  title = input.required<string>();
}
