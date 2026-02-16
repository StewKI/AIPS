import { SignalRService } from '@/services/signalr.ts'
import type { Arrow, Line, MoveShapeCommand, Rectangle, TextShape, Whiteboard } from '@/types/whiteboard.ts'

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

  async addArrow(arrow: Arrow) {
    await client.invoke('AddArrow', arrow)
  },

  async addLine(line: Line) {
    await client.invoke('AddLine', line)
  },

  async addTextShape(textShape: TextShape) {
    await client.invoke('AddTextShape', textShape)
  },

  onInitWhiteboard(callback: (whiteboard: Whiteboard) => void) {
    client.on<Whiteboard>('InitWhiteboard', callback)
  },

  onAddedRectangle(callback: (rectangle: Rectangle) => void) {
    client.on<Rectangle>('AddedRectangle', callback)
  },

  onAddedArrow(callback: (arrow: Arrow) => void) {
    client.on<Arrow>('AddedArrow', callback)
  },

  onAddedLine(callback: (line: Line) => void) {
    client.on<Line>('AddedLine', callback)
  },

  onAddedTextShape(callback: (textShape: TextShape) => void) {
    client.on<TextShape>('AddedTextShape', callback)
  },

  async moveShape(command: MoveShapeCommand) {
    await client.invoke('MoveShape', command)
  },

  async placeShape(command: MoveShapeCommand) {
    await client.invoke('PlaceShape', command)
  },

  onMovedShape(callback: (command: MoveShapeCommand) => void) {
    client.on<MoveShapeCommand>('MovedShape', callback)
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
    client.off('AddedArrow')
    client.off('AddedLine')
    client.off('AddedTextShape')
    client.off('MovedShape')
    client.off('Joined')
    client.off('Leaved')
  },
}
