using Godot;

[Tool]
[GlobalClass]
public partial class Interactable : Area2D
{
  [Export]
  public Popup Popup { get; set; }

  public override void _Ready()
  {
    base._Ready();
    BodyEntered += OnBodyEntered;
    BodyExited += OnBodyExited;
  }

  private void OnBodyEntered(Node2D body)
  {
    // GD.Print("body entered interaction zone");
    if (body.GetType() == typeof(InteractionArea))
    {
      //  connect interaction
      ((InteractionArea)body).ConnectInteractable(this);

      //  animate icon
      Popup.On();
    }
  }

  private void OnBodyExited(Node2D body)
  {
    // GD.Print("body exited interaction zone");
    if (body.GetType() == typeof(InteractionArea))
    {
      //  connect interaction
      ((InteractionArea)body).DisconnectInteractable(this);

      //  animate icon
      Popup.Off();
    }
  }
}