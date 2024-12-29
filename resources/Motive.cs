using System;
using Godot;

public partial class Motive : Resource
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public MotiveStrength Strength { get; set; }
    public MotiveDesire Desire { get; set; }
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