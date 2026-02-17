namespace AipsCore.Domain.Models.Shape.Sub.Arrow;

public partial class Arrow
{
    public override void Move(int newPositionX, int newPositionY)
    {
        EndPosition.X += newPositionX - Position.X;
        EndPosition.Y += newPositionY - Position.Y;
        base.Move(newPositionX, newPositionY);
    }
}