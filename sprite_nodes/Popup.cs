using Godot;

[Tool]
public partial class Popup : Sprite2D
{
  [Export]
  public float PopDistance { get; set; } = 64f;

  private Sprite2D icon;
  private Tween iconAnimation;
  private Timer iconAnimationTimer;
  private float iconAnimationDuration = 0.1f;

  public override void _Ready()
  {
    base._Ready();

    iconAnimationTimer = GetNode<Timer>("Timer");
    iconAnimationTimer.Timeout += OnAnimationEnd;
  }

  public void On()
  {
    Visible = true;
    iconAnimation = CreateTween();
    iconAnimation.TweenProperty(this, "position", Position + new Vector2(0f, -PopDistance), iconAnimationDuration);
  }

  public void Off()
  {
    iconAnimation = CreateTween();
    iconAnimation.TweenProperty(this, "position", Position + new Vector2(0f, PopDistance), iconAnimationDuration);
    iconAnimationTimer.Start(iconAnimationDuration);
  }

  private void OnAnimationEnd()
  {
    Visible = false;
  }
}
