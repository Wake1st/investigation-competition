using System;
using Godot;

[GlobalClass]
public partial class Action : Resource
{
    [Export]
    public string Term { get; set; }
    [Export]
    public string Description { get; set; }

    public Guid Id { get; set; } = Guid.NewGuid();
}