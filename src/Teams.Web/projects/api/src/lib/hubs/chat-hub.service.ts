import { inject, Injectable, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { API_CONFIG } from '../api-config';
import { ChannelDto, MessageDto } from '../models';

export interface ChatUserEvent {
  userId: string;
  displayName: string;
  channelId: string;
}

export interface ChatTypingEvent {
  userId: string;
  displayName: string;
  channelId: string;
}

export interface ChannelCreatedEvent {
  channelId: string;
  channelName: string;
  description: string;
  createdByDisplayName: string;
  createdAt: string;
}

@Injectable({ providedIn: 'root' })
export class ChatHubService {
  private readonly config = inject(API_CONFIG);
  private connection: signalR.HubConnection | null = null;

  readonly messageReceived = signal<MessageDto | null>(null);
  readonly userJoinedChannel = signal<ChatUserEvent | null>(null);
  readonly userLeftChannel = signal<ChatUserEvent | null>(null);
  readonly userTyping = signal<ChatTypingEvent | null>(null);
  readonly channelCreated = signal<ChannelCreatedEvent | null>(null);
  readonly connected = signal(false);

  async start(): Promise<void> {
    if (this.connection) return;

    const builder = new signalR.HubConnectionBuilder()
      .withUrl(`${this.config.baseUrl}/hubs/chat`, {
        accessTokenFactory: this.config.accessTokenFactory ?? (() => ''),
      })
      .withAutomaticReconnect();

    this.connection = builder.build();

    this.connection.on('ReceiveMessage', (message: MessageDto) => {
      this.messageReceived.set(message);
    });

    this.connection.on('UserJoinedChannel', (event: ChatUserEvent) => {
      this.userJoinedChannel.set(event);
    });

    this.connection.on('UserLeftChannel', (event: ChatUserEvent) => {
      this.userLeftChannel.set(event);
    });

    this.connection.on('UserTyping', (event: ChatTypingEvent) => {
      this.userTyping.set(event);
    });

    this.connection.on('ChannelCreated', (event: ChannelCreatedEvent) => {
      this.channelCreated.set(event);
    });

    this.connection.onclose(() => this.connected.set(false));
    this.connection.onreconnected(() => this.connected.set(true));

    await this.connection.start();
    this.connected.set(true);
  }

  async stop(): Promise<void> {
    if (!this.connection) return;
    await this.connection.stop();
    this.connection = null;
    this.connected.set(false);
  }

  async joinChannel(channelId: string): Promise<void> {
    await this.connection?.invoke('JoinChannel', channelId);
  }

  async leaveChannel(channelId: string): Promise<void> {
    await this.connection?.invoke('LeaveChannel', channelId);
  }

  async sendMessage(channelId: string, content: string): Promise<void> {
    await this.connection?.invoke('SendMessage', channelId, content);
  }

  async startTyping(channelId: string): Promise<void> {
    await this.connection?.invoke('StartTyping', channelId);
  }
}
