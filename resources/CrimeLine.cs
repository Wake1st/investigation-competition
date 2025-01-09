using System;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class CrimeLine : Resource
{
  [Export]
  public Occurence RootNode { get; set; }

  public Guid Id { get; set; } = Guid.NewGuid();
  public Dictionary<Person, Timeline> SuspectTimelines { get; set; }
}
