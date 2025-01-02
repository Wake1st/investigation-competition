using Godot;

[Tool]
public partial class ClueNode : Node2D
{
  [Export]
  public Clue Clue { get; set; }
  [Export]
  public Texture2D Image
  {
    get => image;
    set
    {
      image = value;
      if (HasNode("Image"))
        GetNode<Sprite2D>("Image").Texture = value;
    }
  }
  private Texture2D image;
}