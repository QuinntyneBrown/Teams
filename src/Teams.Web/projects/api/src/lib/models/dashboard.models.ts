import { MeetingDto } from './meeting.models';
import { ActivityFeedItemDto } from './notification.models';
import { TeamMemberDto, TimeZoneGroupCardDto } from './team.models';

export interface DashboardResult {
  userId: string;
  upcomingMeetings: MeetingDto[];
  recentActivity: ActivityFeedItemDto[];
  teamTimeZones: TimeZoneGroupCardDto[];
  onlineTeamMemberCount: number;
  totalTeamMemberCount: number;
  generatedAt: string;
}
