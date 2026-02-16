export interface Position {
  x: number
  y: number
}

export interface Shape {
  id: string
  ownerId: string
  position: Position
  color: string
}

export interface Rectangle extends Shape {
  endPosition: Position
  borderThickness: number
}

export interface Arrow extends Shape {
  endPosition: Position
  thickness: number
}

export interface Line extends Shape {
  endPosition: Position
  thickness: number
}

export interface TextShape extends Shape {
  textValue: string
  textSize: number
}

export interface Whiteboard {
  whiteboardId: string
  ownerId: string
  rectangles: Rectangle[]
  arrows: Arrow[]
  lines: Line[]
  textShapes: TextShape[]
}

export interface MoveShapeCommand {
  shapeId: string
  newPositionX: number
  newPositionY: number
}

export type ShapeTool = 'hand' | 'rectangle' | 'arrow' | 'line' | 'text'
export type ShapeType = 'rectangle' | 'arrow' | 'line' | 'textShape'
