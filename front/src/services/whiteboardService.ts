import type {JoinResult, Whiteboard} from "@/types";
import {api} from './api'
import type {WhiteboardJoinPolicy} from "@/enums";

export const whiteboardService = {
  async getWhiteboardHistory(): Promise<Whiteboard[]> {
    const raw = await api.get<any[]>('/api/Whiteboard/history')
    return raw.map(mapWhiteboard)
  },

  async getRecentWhiteboards(): Promise<Whiteboard[]> {
    const raw = await api.get<any[]>('/api/Whiteboard/recent')
    return raw.map(mapWhiteboard)
  },

  async createNewWhiteboard(title: string, joinPolicy: WhiteboardJoinPolicy, maxParticipants: number): Promise<string> {
    return await api.post<string>('/api/Whiteboard', { title: title, maxParticipants: maxParticipants, joinPolicy: joinPolicy})
  },

  async getWhiteboardById(id: string): Promise<Whiteboard> {
    return await api.get<any>(`/api/Whiteboard/${id}`).then(mapWhiteboard)
  },

  async deleteWhiteboard(id: string): Promise<void> {
    await api.delete(`/api/Whiteboard/${id}`)
  },

  async joinWhiteboard(code: string): Promise<JoinResult> {
    const raw = await api.post<any>(`/api/Whiteboard/join`, {code: code})
    return {
      whiteboardId: raw.whiteboardId,
      status: raw.status
    }
  }
}

function mapWhiteboard(raw: any): Whiteboard {
  return {
    id: raw.id,
    ownerId: raw.ownerId,
    code: raw.code,
    title: raw.title,
    createdAt: new Date(raw.createdAt),
    deletedAt: raw.deletedAt ? new Date(raw.deletedAt) : undefined,
    maxParticipants: raw.maxParticipants,
    joinPolicy: raw.joinPolicy,
    state: raw.state,
  }
}
