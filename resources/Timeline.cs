using System;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Timeline : Resource
{
    [Export]
    public Array<Occurence> Nodes { get; set; }

    public Guid Id { get; set; } = Guid.NewGuid();
}
