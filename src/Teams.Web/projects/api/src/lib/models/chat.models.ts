export interface ChannelDto {
  id: string;
  name: string;
  description: string;
  createdAt: string;
  createdByUserId: string;
}

export interface MessageDto {
  id: string;
  channelId: string;
  senderId: string;
  senderDisplayName: string;
  content: string;
  sentAt: string;
  editedAt: string | null;
  isDeleted: boolean;
}

export interface CreateChannelRequest {
  name: string;
  description: string;
  createdByUserId: string;
  createdByDisplayName: string;
}

export interface SendMessageRequest {
  senderId: string;
  senderDisplayName: string;
  content: string;
}
