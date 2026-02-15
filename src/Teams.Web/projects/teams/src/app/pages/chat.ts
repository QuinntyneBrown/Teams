import { Component, computed, effect, inject, OnDestroy, OnInit, signal } from '@angular/core';
import {
  SectionHeader,
  SearchBar,
  IconButton,
  ChannelListItem,
  ChatMessage,
  MessageInput,
  Icon,
} from 'components';
import { ChatService, ChatHubService, ChannelDto, MessageDto } from 'api';
import { AuthStore } from '../services/auth-store';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-chat',
  imports: [SectionHeader, SearchBar, IconButton, ChannelListItem, ChatMessage, MessageInput, Icon, DatePipe],
  template: `
    <div class="chat-page" [class.show-messages]="showMessages()">
      <!-- Channel sidebar -->
      <div class="channel-panel">
        <lib-section-header title="Channels">
          <lib-icon-button [size]="32" (clicked)="onAddChannel()">
            <lib-icon name="plus" [size]="16" />
          </lib-icon-button>
        </lib-section-header>

        <lib-search-bar placeholder="Search channels..." [(value)]="channelSearch" />

        <div class="channel-list">
          @for (ch of filteredChannels(); track ch.id) {
            <lib-channel-list-item
              [name]="ch.name"
              [active]="activeChannel()?.id === ch.id"
              (clicked)="selectChannel(ch)"
            />
          }
        </div>
      </div>

      <!-- Message area -->
      <div class="message-panel">
        <div class="msg-header">
          <button class="back-btn" (click)="showMessages.set(false)">
            <lib-icon name="chevron-left" [size]="20" />
          </button>
          <span class="channel-name"># {{ activeChannel()?.name ?? 'Select a channel' }}</span>
        </div>

        <div class="msg-list">
          @for (msg of messages(); track msg.id) {
            <lib-chat-message
              [isMine]="msg.senderId === auth.userId()"
              [initials]="getInitials(msg.senderDisplayName)"
              [senderName]="msg.senderDisplayName"
              [message]="msg.content"
              [time]="(msg.sentAt | date:'shortTime') ?? ''"
            />
          }
        </div>

        <lib-message-input (messageSent)="sendMessage($event)" />
      </div>
    </div>
  `,
  styles: `
    :host { display: block; height: 100%; }
    .chat-page {
      display: flex;
      height: 100%;
    }
    .channel-panel {
      display: flex;
      flex-direction: column;
      gap: 16px;
      width: 280px;
      padding: 24px 20px;
      border-right: 1px solid var(--ts-border-subtle, #f3f4f6);
      flex-shrink: 0;
      overflow-y: auto;
    }
    .channel-list {
      display: flex;
      flex-direction: column;
      gap: 2px;
    }
    .message-panel {
      flex: 1;
      display: flex;
      flex-direction: column;
      min-width: 0;
    }
    .msg-header {
      display: flex;
      align-items: center;
      gap: 12px;
      height: 60px;
      padding: 0 24px;
      border-bottom: 1px solid var(--ts-border-subtle, #f3f4f6);
      flex-shrink: 0;
    }
    .back-btn {
      display: none;
      align-items: center;
      justify-content: center;
      background: none;
      border: none;
      cursor: pointer;
      padding: 0;
      color: var(--ts-text-primary);
    }
    .channel-name {
      font-family: var(--ts-font-heading, 'Bricolage Grotesque', sans-serif);
      font-size: 16px;
      font-weight: 700;
      color: var(--ts-text-primary, #1a1a1a);
    }
    .msg-list {
      flex: 1;
      overflow-y: auto;
      display: flex;
      flex-direction: column;
      gap: 16px;
      padding: 20px 24px;
    }

    @media (max-width: 767px) {
      .channel-panel {
        width: 100%;
        border-right: none;
      }
      .message-panel {
        display: none;
      }
      .chat-page.show-messages .channel-panel {
        display: none;
      }
      .chat-page.show-messages .message-panel {
        display: flex;
      }
      .chat-page.show-messages .back-btn {
        display: flex;
      }
    }
  `,
})
export class ChatPage implements OnInit, OnDestroy {
  private readonly chatService = inject(ChatService);
  private readonly chatHub = inject(ChatHubService);
  readonly auth = inject(AuthStore);

  protected readonly channels = signal<ChannelDto[]>([]);
  protected readonly activeChannel = signal<ChannelDto | null>(null);
  protected readonly messages = signal<MessageDto[]>([]);
  protected readonly channelSearch = signal('');
  protected readonly showMessages = signal(false);

  protected readonly filteredChannels = computed(() => {
    const q = this.channelSearch().toLowerCase();
    if (!q) return this.channels();
    return this.channels().filter((ch) => ch.name.toLowerCase().includes(q));
  });

  constructor() {
    effect(() => {
      const msg = this.chatHub.messageReceived();
      if (msg && msg.channelId === this.activeChannel()?.id) {
        this.messages.update((prev) => [...prev, msg]);
      }
    });
  }

  ngOnInit() {
    this.chatService.getChannels().subscribe((channels) => {
      this.channels.set(channels);
      if (channels.length > 0) {
        this.selectChannel(channels[0]);
      }
    });
    this.chatHub.start().catch(() => {});
  }

  ngOnDestroy() {
    this.chatHub.stop().catch(() => {});
  }

  selectChannel(ch: ChannelDto) {
    this.activeChannel.set(ch);
    this.showMessages.set(true);
    this.chatService.getMessages(ch.id).subscribe((msgs) => {
      this.messages.set(msgs);
    });
  }

  sendMessage(content: string) {
    const ch = this.activeChannel();
    if (!ch) return;
    this.chatService
      .sendMessage(ch.id, {
        senderId: this.auth.userId(),
        senderDisplayName: this.auth.displayName(),
        content,
      })
      .subscribe((msg) => {
        this.messages.update((prev) => [...prev, msg]);
      });
  }

  onAddChannel() {
    // placeholder for channel creation
  }

  getInitials(name: string): string {
    return name
      .split(' ')
      .map((p) => p[0])
      .join('')
      .toUpperCase()
      .slice(0, 2);
  }
}
