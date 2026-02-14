import { Component, input, model } from '@angular/core';
import { Icon } from './icon';

@Component({
  selector: 'lib-search-bar',
  imports: [Icon],
  template: `
    <div class="search-bar">
      <lib-icon name="search" [size]="18" />
      <input
        type="text"
        [placeholder]="placeholder()"
        [value]="value()"
        (input)="onInput($event)"
      />
    </div>
  `,
  styles: `
    :host {
      display: block;
    }
    .search-bar {
      display: flex;
      align-items: center;
      gap: 12px;
      height: 48px;
      padding: 0 18px;
      border-radius: 24px;
      background: var(--ts-bg-card, #f6f7f8);
      color: var(--ts-text-tertiary, #9ca3af);
    }
    input {
      flex: 1;
      border: none;
      background: transparent;
      outline: none;
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 14px;
      color: var(--ts-text-primary, #1a1a1a);
    }
    input::placeholder {
      color: var(--ts-text-tertiary, #9ca3af);
    }
  `,
})
export class SearchBar {
  placeholder = input('Search...');
  value = model('');

  onInput(event: Event) {
    this.value.set((event.target as HTMLInputElement).value);
  }
}
