using System;
using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class CrimeLine : Resource
{
  [Export]
  public TimelineNode RootNode { get; set; }
  public List<TimelineNode> Nodes { get; set; }

  public Guid Id { get; set; } = Guid.NewGuid();
}
