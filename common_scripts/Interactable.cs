using System.Runtime.CompilerServices;
using Godot;

public partial class Interactable : Area2D
{
  [Export]
  public Clue Clue { get; set; }

  private Sprite2D icon;
  private Tween iconAnimation;
  private Timer iconAnimationTimer;
  private float iconAnimationDuration = 0.1f;

  public override void _Ready()
  {
    base._Ready();
    BodyEntered += OnBodyEntered;
    BodyExited += OnBodyExited;

    icon = GetNode<Sprite2D>("Icon");

    iconAnimationTimer = GetNode<Timer>("Icon/Timer");
    iconAnimationTimer.Timeout += OnAnimationEnd;
  }

  private void OnBodyEntered(Node2D body)
  {
    // GD.Print("body entered interaction zone");
    if (body.GetType() == typeof(InteractionArea))
    {
      //  connect interaction
      ((InteractionArea)body).ConnectInteractable(this);

      //  animate icon
      icon.Visible = true;
      iconAnimation = CreateTween();
      iconAnimation.TweenProperty(icon, "position", icon.Position + new Vector2(0f, -64f), iconAnimationDuration);
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
      iconAnimation = CreateTween();
      iconAnimation.TweenProperty(icon, "position", icon.Position + new Vector2(0f, 64f), iconAnimationDuration);
      iconAnimationTimer.Start(iconAnimationDuration);
    }
  }

  private void OnAnimationEnd()
  {
    icon.Visible = false;
  }

  public void Destroy()
  {
    GD.Print($"has timer: {HasNode("Icon/Timer")}");
    QueueFree();
  }
}