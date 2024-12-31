using System;
using Godot;

[GlobalClass]
public partial class Action : Resource
{
    [Export]
    public Person Actor { get; set; }

    public Guid Id { get; set; } = Guid.NewGuid();
}