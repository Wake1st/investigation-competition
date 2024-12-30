using System;
using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class Timeline : Resource
{
    [Export]
    public string Name { get; set; }
    [Export]
    public bool SinglePath { get; set; }
    [Export]
    public TimelineNode RootNode { get; set; }
    public List<TimelineNode> Nodes { get; set; }

    public Guid Id { get; set; } = Guid.NewGuid();
}
