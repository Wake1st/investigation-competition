using System;
using Godot;

public partial class Suspect : Resource
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public Motive Motive { get; set; }
    public Timeline Timeline { get; set; }
}