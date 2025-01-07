using System;
using Godot;

[GlobalClass]
public partial class Suspect : Resource
{
    [Export]
    public Person Person { get; set; }
    [Export]
    public Motive Motive { get; set; }
    [Export]
    public Timeline Timeline { get; set; }

    public Guid Id { get; set; } = Guid.NewGuid();
}