using Godot;

public partial class Interactable : Area2D
{
  [Export]
  public Clue Clue { get; set; }

  private Sprite2D icon;

  public override void _Ready()
  {
    base._Ready();
    icon = GetNode<Sprite2D>("Icon");
    BodyEntered += OnBodyEntered;
    BodyExited += OnBodyExited;
  }

  private void OnBodyEntered(Node2D body)
  {
    GD.Print("body entered interaction zone");
    if (body.GetType() == typeof(InteractionArea))
    {
      //  connect interaction
      ((InteractionArea)body).ConnectInteractable(this);

      //  animate icon
      icon.Visible = true;
    }
  }

  private void OnBodyExited(Node2D body)
  {
    GD.Print("body exited interaction zone");
    if (body.GetType() == typeof(InteractionArea))
    {
      //  connect interaction
      ((InteractionArea)body).DisconnectInteractable(this);

      //  animate icon
      icon.Visible = false;
    }
  }
}