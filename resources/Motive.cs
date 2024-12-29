using System;
using Godot;

[GlobalClass]
public partial class Motive : Resource
{
    [Export]
    public MotiveStrength Strength { get; set; }
    [Export]
    public MotiveDesire Desire { get; set; }

    public Guid Id { get; set; } = Guid.NewGuid();
}

public enum MotiveStrength
{
    Weak,
    Fair,
    Strong,
}

public enum MotiveDesire
{
    Money,
    Power,
    Love,
    Revenge,
}