import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', loadComponent: () => import('./pages/home').then(m => m.HomePage) },
  { path: 'chat', loadComponent: () => import('./pages/chat').then(m => m.ChatPage) },
  { path: 'meetings', loadComponent: () => import('./pages/meetings').then(m => m.MeetingsPage) },
  { path: 'team', loadComponent: () => import('./pages/team').then(m => m.TeamPage) },
];
