import { inject, Injectable, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { API_CONFIG } from '../api-config';

export interface MeetingCreatedEvent {
  meetingId: string;
  title: string;
  startTimeUtc: string;
  endTimeUtc: string;
  organizerDisplayName: string;
}

export interface MeetingReminderEvent {
  meetingId: string;
  title: string;
  startTimeUtc: string;
  minutesUntilStart: number;
  meetingLink: string;
}

@Injectable({ providedIn: 'root' })
export class MeetingHubService {
  private readonly config = inject(API_CONFIG);
  private connection: signalR.HubConnection | null = null;

  readonly meetingCreated = signal<MeetingCreatedEvent | null>(null);
  readonly meetingReminder = signal<MeetingReminderEvent | null>(null);
  readonly connected = signal(false);

  async start(): Promise<void> {
    if (this.connection) return;

    const builder = new signalR.HubConnectionBuilder()
      .withUrl(`${this.config.baseUrl}/hubs/meetings`, {
        accessTokenFactory: this.config.accessTokenFactory ?? (() => ''),
      })
      .withAutomaticReconnect();

    this.connection = builder.build();

    this.connection.on('MeetingCreated', (event: MeetingCreatedEvent) => {
      this.meetingCreated.set(event);
    });

    this.connection.on('MeetingReminder', (event: MeetingReminderEvent) => {
      this.meetingReminder.set(event);
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

  async joinMeeting(meetingId: string): Promise<void> {
    await this.connection?.invoke('JoinMeeting', meetingId);
  }

  async leaveMeeting(meetingId: string): Promise<void> {
    await this.connection?.invoke('LeaveMeeting', meetingId);
  }

  async subscribeToSchedule(): Promise<void> {
    await this.connection?.invoke('SubscribeToSchedule');
  }
}
