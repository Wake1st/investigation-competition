using System;
using Godot;

[GlobalClass]
public partial class Suspect : Resource
{
    [Export]
    public string Name { get; set; }
    [Export(PropertyHint.MultilineText)]
    public string Description { get; set; }
    [Export]
    public Motive Motive { get; set; }
    [Export]
    public Timeline Timeline { get; set; }

    public Guid Id { get; set; } = Guid.NewGuid();
}