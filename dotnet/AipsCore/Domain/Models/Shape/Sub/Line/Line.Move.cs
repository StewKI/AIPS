namespace AipsCore.Domain.Models.Shape.Sub.Line;

public partial class Line
{
    public override void Move(int newPositionX, int newPositionY)
    {
        EndPosition.X += newPositionX - Position.X;
        EndPosition.Y += newPositionY - Position.Y;
        base.Move(newPositionX, newPositionY);
    }
}