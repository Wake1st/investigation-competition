using Godot;

public partial class EvidenceSlot : Panel
{
  const float MIN_EXPANDED_X = 128f;
  const float MAX_EXPANDED_X = 512f;
  const float COLLAPSE_TIMER_DELAY = 0.2f;

  [Signal]
  public delegate void SlotResizedEventHandler();

  [Export]
  public float expantionAnimationDuration = 0.1f;
  private Tween expansionAnimation;
  private Timer collapseTimer;
  private bool willCollapse = false;

  public override void _Ready()
  {
    collapseTimer = GetNode<Timer>("CollapseDelay");
    collapseTimer.Timeout += OnCollapseDelayTimeout;

    MouseEntered += OnMouseEnter;
    MouseExited += OnMouseExit;
  }

  private void OnMouseEnter()
  {
    expansionAnimation = CreateTween();
    expansionAnimation.TweenProperty(this, "custom_minimum_size:x", MAX_EXPANDED_X, expantionAnimationDuration);
  }

  private void OnMouseExit()
  {
    // if (collapseTimer.IsStopped() && !willCollapse)
    if (new Rect2(GlobalPosition, Size).HasPoint(GetGlobalMousePosition()))
    {
      GD.Print($"position: {GlobalPosition}\tsize: {Size}\tmouse: {GetGlobalMousePosition()}");
      expansionAnimation = CreateTween();
      expansionAnimation.TweenProperty(this, "custom_minimum_size:x", MIN_EXPANDED_X, expantionAnimationDuration);
      // collapseTimer.Start(COLLAPSE_TIMER_DELAY);
      // willCollapse = true;
    }
  }

  private void OnCollapseDelayTimeout()
  {
    expansionAnimation = CreateTween();
    expansionAnimation.TweenProperty(this, "custom_minimum_size:x", MIN_EXPANDED_X, expantionAnimationDuration);
    willCollapse = false;
  }

  private void OnRectResized()
  {
    EmitSignal(SignalName.SlotResized);
  }
}
