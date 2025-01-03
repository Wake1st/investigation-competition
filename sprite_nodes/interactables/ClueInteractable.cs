using Godot;

[Tool]
[GlobalClass]
public partial class ClueInteractable : Interactable
{
  [Export]
  public ClueNode ClueNode { get; set; }

  public void Destroy()
  {
    GD.Print($"has timer: {HasNode("Icon/Timer")}");
    QueueFree();
  }
}
