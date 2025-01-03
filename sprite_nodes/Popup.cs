using Godot;

public partial class Popup : Sprite2D
{

  private Sprite2D icon;
  private Tween iconAnimation;
  private Timer iconAnimationTimer;
  private float iconAnimationDuration = 0.1f;

  public override void _Ready()
  {
    base._Ready();

    icon = GetNode<Sprite2D>("Icon");

    iconAnimationTimer = GetNode<Timer>("Icon/Timer");
    iconAnimationTimer.Timeout += OnAnimationEnd;
  }

  public void On()
  {
    icon.Visible = true;
    iconAnimation = CreateTween();
    iconAnimation.TweenProperty(icon, "position", icon.Position + new Vector2(0f, -64f), iconAnimationDuration);
  }

  public void Off()
  {
    iconAnimation = CreateTween();
    iconAnimation.TweenProperty(icon, "position", icon.Position + new Vector2(0f, 64f), iconAnimationDuration);
    iconAnimationTimer.Start(iconAnimationDuration);
  }

  private void OnAnimationEnd()
  {
    icon.Visible = false;
  }
}
