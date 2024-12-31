using System;
using Godot;

[GlobalClass]
public partial class Action : Resource
{
    [Export]
    public Person Perpetrator { get; set; }
    
    public Guid Id { get; set; } = Guid.NewGuid();
}