using Godot;
using Godot.Collections;

[GlobalClass]
public partial class GroupAction : Action
{
  [Export]
  public GroupActionType Type { get; set; }
  [Export]
  public Array<Person> Others { get; set; }
}

public enum GroupActionType
{
  Eating,
  Drinking,
  Dancing,
  Discussing,
  Performing,
  Playing
}