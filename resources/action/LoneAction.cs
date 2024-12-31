using Godot;

[GlobalClass]
public partial class LoneAction : Action
{
  [Export]
  public LoneActionType Type { get; set; }
}

public enum LoneActionType
{
  Drinking,
  Eating,
  Resting,
  Reading,
  Walking,
}