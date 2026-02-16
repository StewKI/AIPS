import { SignalRService } from '@/services/signalr.ts'
import type { Rectangle, Whiteboard } from '@/types/whiteboard.ts'

const client = new SignalRService(`/hubs/whiteboard`)

export const whiteboardHubService = {
  async connect() {
    await client.start()
  },

  async disconnect() {
    await client.stop()
  },

  async joinWhiteboard(id: string) {
    await client.invoke('JoinWhiteboard', id)
  },

  async leaveWhiteboard(id: string) {
    await client.invoke('LeaveWhiteboard', id)
  },

  async addRectangle(rectangle: Rectangle) {
    await client.invoke('AddRectangle', rectangle)
  },

  onInitWhiteboard(callback: (whiteboard: Whiteboard) => void) {
    client.on<Whiteboard>('InitWhiteboard', callback)
  },

  onAddedRectangle(callback: (rectangle: Rectangle) => void) {
    client.on<Rectangle>('AddedRectangle', callback)
  },

  onJoined(callback: (userId: string) => void) {
    client.on<string>('Joined', callback)
  },

  onLeaved(callback: (userId: string) => void) {
    client.on<string>('Leaved', callback)
  },

  offAll() {
    client.off('InitWhiteboard')
    client.off('AddedRectangle')
    client.off('Joined')
    client.off('Leaved')
  },
}
