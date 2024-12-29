using System;
using Godot;

public partial class Action : GodotObject
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Term { get; set; }
    public string Description { get; set; }
}