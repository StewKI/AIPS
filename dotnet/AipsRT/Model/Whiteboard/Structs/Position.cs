namespace AipsRT.Model.Whiteboard.Structs;

public struct Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Position operator -(Position position, Position otherPosition)
    {
        return new Position(position.X - otherPosition.X, position.Y - otherPosition.Y);
    }
    
    public static Position operator +(Position position, Position otherPosition)
    {
        return new Position(position.X + otherPosition.X, position.Y + otherPosition.Y);
    }
}