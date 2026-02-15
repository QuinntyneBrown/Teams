export interface TeamMemberDto {
  id: string;
  displayName: string;
  email: string;
  role: string;
  initials: string;
  avatarColor: string;
  timeZoneId: string;
  status: string;
  lastSeenAt: string;
  joinedAt: string;
}

export interface TeamInvitationDto {
  id: string;
  email: string;
  invitedByUserId: string;
  status: string;
  createdAt: string;
}

export interface TimeZoneGroupCardDto {
  timeZoneId: string;
  displayLabel: string;
  currentLocalTime: string;
  utcOffset: string;
  members: TeamMemberDto[];
}

export interface UpdatePresenceRequest {
  status: string;
  statusMessage?: string;
}

export interface InviteMemberRequest {
  email: string;
  invitedByUserId: string;
  personalMessage?: string;
}
