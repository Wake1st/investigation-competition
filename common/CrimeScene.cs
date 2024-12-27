using System.Collections.Generic;
using Godot;

public partial class CrimeScene : GodotObject {
    public Location Location { get; set; }
    public List<Connection> Connections { get; set; }
    public List<Suspect> Suspects { get; set; }
    public List<Clue> Clues { get; set; }
}

public partial class Connection : GodotObject {
    public CrimeScene Neighbor { get; set; }
    public Direction Direction { get; set; }
}

public enum Direction {
    Up,
    Down,
    Right,
    Left,
    Front,
    Back
}