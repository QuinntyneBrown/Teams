import { inject, Injectable, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { API_CONFIG } from '../api-config';
import { NotificationDto } from '../models';

@Injectable({ providedIn: 'root' })
export class NotificationHubService {
  private readonly config = inject(API_CONFIG);
  private connection: signalR.HubConnection | null = null;

  readonly meetingReminder = signal<NotificationDto | null>(null);
  readonly connected = signal(false);

  async start(): Promise<void> {
    if (this.connection) return;

    const builder = new signalR.HubConnectionBuilder()
      .withUrl(`${this.config.baseUrl}/hubs/notifications`, {
        accessTokenFactory: this.config.accessTokenFactory ?? (() => ''),
      })
      .withAutomaticReconnect();

    this.connection = builder.build();

    this.connection.on('MeetingReminder', (notification: NotificationDto) => {
      this.meetingReminder.set(notification);
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
}
