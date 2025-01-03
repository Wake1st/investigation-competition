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
}
