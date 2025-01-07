using Godot;

[GlobalClass]
public partial class Person : Resource {
    [Export]
    public string Name { get; set; }
    [Export(PropertyHint.MultilineText)]
    public string Description { get; set; }
}