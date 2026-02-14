import { Component, input } from '@angular/core';
import { Avatar } from './avatar';

@Component({
  selector: 'lib-chat-message',
  imports: [Avatar],
  template: `
    @if (!isMine()) {
      <div class="message incoming">
        <lib-avatar
          [initials]="initials()"
          [size]="32"
          [bgColor]="avatarBg()"
          [textColor]="avatarColor()"
          [fontSize]="12"
        />
        <div class="body">
          <div class="header">
            <span class="name">{{ senderName() }}</span>
            @if (flag()) {
              <span class="flag">{{ flag() }}</span>
            }
            <span class="time">{{ time() }}</span>
          </div>
          <div class="bubble incoming-bubble">
            <span class="text">{{ message() }}</span>
          </div>
        </div>
      </div>
    } @else {
      <div class="message outgoing">
        <div class="body-right">
          <div class="bubble outgoing-bubble">
            <span class="text-white">{{ message() }}</span>
          </div>
          <div class="time-row">
            <span class="time">{{ time() }}</span>
          </div>
        </div>
      </div>
    }
  `,
  styles: `
    :host {
      display: block;
    }
    .message {
      display: flex;
      width: 100%;
    }
    .incoming {
      gap: 10px;
    }
    .outgoing {
      justify-content: flex-end;
    }
    .body {
      display: flex;
      flex-direction: column;
      gap: 4px;
      min-width: 0;
      flex: 1;
    }
    .body-right {
      display: flex;
      flex-direction: column;
      gap: 4px;
    }
    .header {
      display: flex;
      align-items: center;
      gap: 8px;
    }
    .name {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 13px;
      font-weight: 600;
      color: var(--ts-text-primary, #1a1a1a);
    }
    .flag {
      font-size: 11px;
    }
    .time {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 11px;
      color: var(--ts-text-tertiary, #9ca3af);
    }
    .bubble {
      padding: 10px 14px;
      max-width: 280px;
    }
    .incoming-bubble {
      background: var(--ts-bg-card, #f6f7f8);
      border-radius: 0 16px 16px 16px;
    }
    .outgoing-bubble {
      background: var(--ts-accent-primary, #6366f1);
      border-radius: 16px 0 16px 16px;
    }
    .text {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 14px;
      color: var(--ts-text-primary, #1a1a1a);
      word-wrap: break-word;
    }
    .text-white {
      font-family: var(--ts-font-body, 'DM Sans', sans-serif);
      font-size: 14px;
      color: #ffffff;
      word-wrap: break-word;
    }
    .time-row {
      display: flex;
      justify-content: flex-end;
      padding-top: 4px;
    }
  `,
})
export class ChatMessage {
  initials = input('');
  avatarBg = input('#E0E7FF');
  avatarColor = input('#6366F1');
  senderName = input('');
  flag = input('');
  time = input.required<string>();
  message = input.required<string>();
  isMine = input(false);
}
