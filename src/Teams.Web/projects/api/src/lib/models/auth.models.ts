export interface LoginRequest {
  email: string;
  displayName: string;
}

export interface LoginResponse {
  token: string;
  userId: string;
  displayName: string;
}
