using Godot;

[Tool]
public partial class RoomNode : Node2D
{
  [Export]
  public Location Location { get; set; }
  [Export]
  public Texture2D Background
  {
    get => background;
    set
    {
      background = value;
      if (HasNode("Image"))
        GetNode<Sprite2D>("Image").Texture = value;
    }
  }
  private Texture2D background;

  private SppSchema SppSchema { get; set; }

  public override void _Ready()
  {
    base._Ready();
    SppSchema = GetNode<SppSchema>("SPPSchema");
  }

  public Vector2 ClampWithinBoundaries(Vector2 position) =>
    SppSchema.ClampWithinBoundaries(position);
}
