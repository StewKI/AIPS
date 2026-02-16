import type { Whiteboard } from "@/types";
import { api } from './api'

export const whiteboardService = {
  async getWhiteboardHistory(): Promise<Whiteboard[]> {
    const raw = await api.get<any[]>('/api/Whiteboard/history')
    return raw.map(mapWhiteboard)
  },

  async getRecentWhiteboards(): Promise<Whiteboard[]> {
    const raw = await api.get<any[]>('/api/Whiteboard/recent')
    return raw.map(mapWhiteboard)
  }
}

function mapWhiteboard(raw: any): Whiteboard {
  return {
    id: raw.id,
    ownerId: raw.ownerId,
    title: raw.title,
    createdAt: new Date(raw.createdAt),
    deletedAt: raw.deletedAt ? new Date(raw.deletedAt) : undefined,
    maxParticipants: raw.maxParticipants,
    joinPolicy: raw.joinPolicy,
    state: raw.state,
  }
}
