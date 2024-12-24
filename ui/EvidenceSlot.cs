using Godot;

public partial class EvidenceSlot : Panel
{
  const float MIN_EXPANDED_X = 64f;
  const float MAX_EXPANDED_X = 256f;

  [Signal]
  public delegate void SlotResizedEventHandler();

  [Export]
  public float expantionAnimationDuration = 0.1f;
  private Tween expansionAnimation;

  private TextureRect Image;
  private RichTextLabel Description;

  public override void _Ready()
  {
    base._Ready();

    Image = GetNode<TextureRect>("Image");
    Description = GetNode<RichTextLabel>("Description");
    GD.Print("added Image, added description");

    MouseEntered += OnMouseEnter;
    MouseExited += OnMouseExit;
  }

  public void Setup(Clue clue)
  {
    Image.Texture = clue.Image;
    Description.Text = clue.Description;
  }

  private void OnMouseEnter()
  {
    expansionAnimation = CreateTween();
    expansionAnimation.TweenProperty(this, "custom_minimum_size:x", MAX_EXPANDED_X, expantionAnimationDuration);
  }

  private void OnMouseExit()
  {
    if (!GetGlobalRect().HasPoint(GetGlobalMousePosition()))
    {
      expansionAnimation = CreateTween();
      expansionAnimation.TweenProperty(this, "custom_minimum_size:x", MIN_EXPANDED_X, expantionAnimationDuration);
    }
  }
}
