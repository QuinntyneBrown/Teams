import { inject, Injectable, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { API_CONFIG } from '../api-config';
import { TeamMemberDto } from '../models';

@Injectable({ providedIn: 'root' })
export class PresenceHubService {
  private readonly config = inject(API_CONFIG);
  private connection: signalR.HubConnection | null = null;

  readonly userOnline = signal<TeamMemberDto | null>(null);
  readonly userOffline = signal<TeamMemberDto | null>(null);
  readonly userStatusChanged = signal<TeamMemberDto | null>(null);
  readonly connected = signal(false);

  async start(): Promise<void> {
    if (this.connection) return;

    const builder = new signalR.HubConnectionBuilder()
      .withUrl(`${this.config.baseUrl}/hubs/presence`, {
        accessTokenFactory: this.config.accessTokenFactory ?? (() => ''),
      })
      .withAutomaticReconnect();

    this.connection = builder.build();

    this.connection.on('UserOnline', (member: TeamMemberDto) => {
      this.userOnline.set(member);
    });

    this.connection.on('UserOffline', (member: TeamMemberDto) => {
      this.userOffline.set(member);
    });

    this.connection.on('UserStatusChanged', (member: TeamMemberDto) => {
      this.userStatusChanged.set(member);
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

  async updateStatus(status: string, statusMessage?: string): Promise<void> {
    await this.connection?.invoke('UpdateStatus', status, statusMessage ?? null);
  }
}
