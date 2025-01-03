using Godot;

[Tool]
[GlobalClass]
public partial class DoorInteractable : Interactable
{
  [Export]
  public bool FlipDoor
  {
    get => GetNode<Sprite2D>("Image").FlipV;
    set
    {
      flipDoor = !flipDoor;
      GetNode<Sprite2D>("Image").FlipV = flipDoor;
    }
  }
  private bool flipDoor;
  [Export]
  public RoomNode CurrentRoom { get; set; }
  [Export]
  public DoorInteractable ConnectingDoor { get; set; }
}
