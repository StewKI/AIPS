import type {JoinResult, Whiteboard} from "@/types";
import {api} from './api'
import type {WhiteboardJoinPolicy} from "@/enums";

export const whiteboardService = {
  async getWhiteboardHistory(): Promise<Whiteboard[]> {
    const result = await api.get<any>('/api/Whiteboard/history')
    return result.whiteboards.map(mapWhiteboard)
  },

  async getRecentWhiteboards(): Promise<Whiteboard[]> {
    const result = await api.get<any>('/api/Whiteboard/recent')
    return result.whiteboards.map(mapWhiteboard)
  },

  async createNewWhiteboard(title: string, joinPolicy: WhiteboardJoinPolicy, maxParticipants: number): Promise<string> {
    const result = await api.post<any>('/api/Whiteboard', { title: title, maxParticipants: maxParticipants, joinPolicy: joinPolicy})
    return result.whiteboardId
  },

  async getWhiteboardById(id: string): Promise<Whiteboard> {
    const result = await api.get<any>(`/api/Whiteboard/${id}`)
    return mapWhiteboard(result)
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
