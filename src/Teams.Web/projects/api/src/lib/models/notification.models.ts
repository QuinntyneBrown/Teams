export enum NotificationType {
  MessageMention = 'MessageMention',
  MeetingReminder = 'MeetingReminder',
  MeetingInvite = 'MeetingInvite',
  MeetingCancellation = 'MeetingCancellation',
  TeamInvite = 'TeamInvite',
  MemberJoined = 'MemberJoined',
  ChannelActivity = 'ChannelActivity',
  DirectMessage = 'DirectMessage',
  SystemAlert = 'SystemAlert',
}

export enum ActivityCategory {
  Chat = 'Chat',
  Meeting = 'Meeting',
  Team = 'Team',
  File = 'File',
}

export interface NotificationDto {
  id: string;
  userId: string;
  title: string;
  description: string;
  type: NotificationType;
  isRead: boolean;
  createdAt: string;
  relatedEntityId: string | null;
}

export interface PaginatedNotificationsDto {
  items: NotificationDto[];
  totalCount: number;
  unreadCount: number;
}

export interface ActivityFeedItemDto {
  id: string;
  actorUserId: string;
  actorDisplayName: string;
  actorInitials: string;
  actorAvatarColor: string;
  action: string;
  description: string;
  createdAt: string;
  category: ActivityCategory;
}
