using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class TimelineNode : Resource
{
  [Export]
  public Occurence Occurence { get; set; }
  public List<Occurence> Neighbors { get; set; }
}