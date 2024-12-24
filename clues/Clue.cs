using Godot;

[Tool]
public partial class Clue : Node2D
{
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

  [Export(PropertyHint.MultilineText)]
  public string Description { get; set; }

  private bool isFoux = false;
}