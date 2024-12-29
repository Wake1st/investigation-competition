using System;
using System.Net.Http.Headers;
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

    public Guid Id { get; set; } = Guid.NewGuid();
    public Timeline Timeline { get; set; }
}