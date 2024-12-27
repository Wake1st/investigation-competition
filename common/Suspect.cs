using Godot;

public partial class Suspect : GodotObject {
    public string Id { get; set; }
    public string Name { get; set; }
    public Motive Motive { get; set; }
    public Timeline Timeline { get; set; }
}