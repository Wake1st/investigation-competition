using Godot;

[Tool]
public partial class AnchorPoint : Node2D
{
  private Vector2 PreviousPosition { get; set; } = new Vector2();

	public Vector2 CheckDelta()
	{
    Vector2 delta = Position - PreviousPosition;
    PreviousPosition = Position;
    return delta;
	}

  public void Reposition(Vector2 position) {
    Position = position;
    PreviousPosition = position;
  }
}
