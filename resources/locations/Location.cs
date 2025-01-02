using System;
using Godot;

[GlobalClass]
public partial class Location : Resource
{
    [Export]
    public string Term { get; set; }
    [Export(PropertyHint.MultilineText)]
    public string Description { get; set; }
    
    public Guid Id { get; set; } = Guid.NewGuid();
}
