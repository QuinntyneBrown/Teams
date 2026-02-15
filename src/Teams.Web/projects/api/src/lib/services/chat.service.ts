import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_CONFIG } from '../api-config';
import { ChannelDto, CreateChannelRequest, MessageDto, SendMessageRequest } from '../models';

@Injectable({ providedIn: 'root' })
export class ChatService {
  private readonly http = inject(HttpClient);
  private readonly config = inject(API_CONFIG);

  getChannels(): Observable<ChannelDto[]> {
    return this.http.get<ChannelDto[]>(`${this.config.baseUrl}/api/chat/channels`);
  }

  createChannel(request: CreateChannelRequest): Observable<ChannelDto> {
    return this.http.post<ChannelDto>(`${this.config.baseUrl}/api/chat/channels`, request);
  }

  getMessages(channelId: string, pageSize = 50, before?: string): Observable<MessageDto[]> {
    let params = new HttpParams().set('pageSize', pageSize);
    if (before) {
      params = params.set('before', before);
    }
    return this.http.get<MessageDto[]>(`${this.config.baseUrl}/api/chat/channels/${channelId}/messages`, { params });
  }

  sendMessage(channelId: string, request: SendMessageRequest): Observable<MessageDto> {
    return this.http.post<MessageDto>(`${this.config.baseUrl}/api/chat/channels/${channelId}/messages`, request);
  }
}
