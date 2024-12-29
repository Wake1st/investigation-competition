using Godot;

[Tool]
public partial class Clue : Node2D
{
  [Export]
  public string Id { get; set; }
  [Export]
  public string Term { get; set; }
  [Export(PropertyHint.MultilineText)]
  public string Description { get; set; }
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


  private bool isFoux = false;
}