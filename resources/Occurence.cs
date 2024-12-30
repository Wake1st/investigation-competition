using System;
using Godot;

[GlobalClass]
public partial class Occurence : Resource
{
    [Export]
    public TimeRange When { get; set; }
    [Export]
    public Location Where { get; set; }
    [Export]
    public Action Cause { get; set; }
    [Export]
    public Action Effect { get; set; }

    public Guid Id { get; set; } = Guid.NewGuid();
}