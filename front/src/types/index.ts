import {type WhiteboardJoinPolicy, WhiteboardState} from "@/enums";

export interface User {
  userId: string
  username: string
  email: string
}

export interface LoginCredentials {
  email: string
  password: string
}

export interface SignupCredentials {
  username: string
  email: string
  password: string
}

export interface AuthResponse {
  accessToken: string
  refreshToken: string
}

export interface Whiteboard {
  id: string
  ownerId: string
  code: string
  title: string
  createdAt: Date
  deletedAt?: Date
  maxParticipants?: number
  joinPolicy?: WhiteboardJoinPolicy
  state: WhiteboardState
}
