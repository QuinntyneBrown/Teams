import { Component, model, output } from '@angular/core';
import { Icon } from './icon';

@Component({
  selector: 'lib-message-input',
  imports: [Icon],
  template: `
    <div class="message-input">
      <button class="attach-btn" (click)="attachClicked.emit()">
        <lib-icon name="plus" [size]="22" />
      </button>
      <div class="input-field">
        <input
          type="text"
          [placeholder]="'Type a message...'"
          [value]="value()"
          (input)="onInput($event)"
          (keydown.enter)="onSend()"
        />
      </div>
      <button class="send-btn" (click)="onSend()">
        <lib-icon name="send" [size]="16" />
      </button>
    </div>
  `,
  styles: `
    :host {
      display: block;
    }
    .message-input {
      display: flex;
      align-items: center;
      gap: 12px;
      padding: 12px 20px 28px 20px;
      background: #ffffff;
      border-top: 1px solid var(--ts-border-subtle, #f3f4f6);
    }
    .attach-btn {
      display: flex;
      align-items: center;
      justify-content: center;
      background: none;
      border: none;
      cursor: pointer;
      padding: 0;
      color: var(--ts-text-secondary, #6b7280);
    }
    .input-field {
      flex: 1;
      display: flex;
      align-items: center;
      height: 40px;
      padding: 0 16px;
      border-radius: 20px;
      background: var(--ts-bg-card, #f6f7f8);
    }
    .input-field input {
      flex: 1;
      border: none;
      background: transparent;
      outline: none;
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 14px;
      color: var(--ts-text-primary, #1a1a1a);
    }
    .input-field input::placeholder {
      color: var(--ts-text-tertiary, #9ca3af);
    }
    .send-btn {
      display: flex;
      align-items: center;
      justify-content: center;
      width: 36px;
      height: 36px;
      border-radius: 18px;
      background: var(--ts-accent-primary, #6366f1);
      border: none;
      cursor: pointer;
      color: #ffffff;
      flex-shrink: 0;
      transition: opacity 0.15s;
    }
    .send-btn:hover {
      opacity: 0.9;
    }
  `,
})
export class MessageInput {
  value = model('');
  messageSent = output<string>();
  attachClicked = output();

  onInput(event: Event) {
    this.value.set((event.target as HTMLInputElement).value);
  }

  onSend() {
    const msg = this.value().trim();
    if (msg) {
      this.messageSent.emit(msg);
      this.value.set('');
    }
  }
}
