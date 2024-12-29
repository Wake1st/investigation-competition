using System;
using Godot;

public partial class Occurence : GodotObject
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public TimeRange When { get; set; }
    public Location Where { get; set; }
    public Action Cause { get; set; }
    public Action Effect { get; set; }
}