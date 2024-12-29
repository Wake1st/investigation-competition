using System;
using Godot;

public partial class Location : GodotObject
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Term { get; set; }
    public string Description { get; set; }
}
