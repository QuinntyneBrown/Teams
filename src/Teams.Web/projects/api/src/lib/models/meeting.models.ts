export interface MeetingDto {
  id: string;
  title: string;
  description: string | null;
  startTimeUtc: string;
  endTimeUtc: string;
  accentColor: string;
  isCancelled: boolean;
  createdByUserId: string;
  attendees: AttendeeDto[];
}

export interface AttendeeDto {
  userId: string;
  displayName: string;
  responseStatus: string;
  timeZoneId: string;
}

export interface CreateMeetingRequest {
  title: string;
  description?: string;
  startTimeUtc: string;
  endTimeUtc: string;
  accentColor?: string;
  createdByUserId: string;
  createdByDisplayName: string;
  timeZone?: string;
  attendees?: AttendeeRequest[];
}

export interface AttendeeRequest {
  userId: string;
  displayName: string;
  timeZoneId?: string;
}

export interface RespondToMeetingRequest {
  userId: string;
  displayName: string;
  response: string;
}

export interface TodayScheduleResult {
  meetings: MeetingDto[];
  timeZoneId: string;
  dayStart: string;
  dayEnd: string;
}

export interface UpcomingMeetingsResult {
  meetings: MeetingDto[];
}

export interface CreateMeetingResult {
  meetingId: string;
  createdAt: string;
}

export interface RespondToMeetingResult {
  success: boolean;
  error: string | null;
}
