using System.Collections.Generic;
using Godot;

public partial class Timeline : GodotObject
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool SinglePath { get; set; }
    public TimelineNode RootNode { get; set; }
    public List<TimelineNode> Nodes { get; set; }
}

public partial class TimelineNode : GodotObject
{
    public Occurence Occurence { get; set; }
    public List<Occurence> Neighbors { get; set; }
}