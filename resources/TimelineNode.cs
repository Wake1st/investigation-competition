using System;
using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class TimelineNode : Resource
{
  [Export]
  public Occurence Occurence { get; set; }

  public Guid Id { get; set; } = Guid.NewGuid();
  public List<Guid> Neighbors { get; set; }
}