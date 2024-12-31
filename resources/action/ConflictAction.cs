using Godot;

[GlobalClass]
public partial class ConflictAction : Action
{
  [Export]
  public ConflictActionType Type { get; set; }
  [Export]
  public Person CoStar { get; set; }
}

public enum ConflictActionType
{
  Arguing,
  LoveMaking,
  Wrestling,
}