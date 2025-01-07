using System.Collections.Generic;
using Godot;

public partial class CrimeScene : Resource
{
    public Location Location { get; set; }
    public List<Room> Rooms { get; set; }
    public List<Clue> Clues { get; set; }
}

public partial class Room : Resource
{
    public Location Location { get; set; }
    public List<Room> Neighbors { get; set; }
}

public enum Direction
{
    Up,
    Down,
    Right,
    Left,
    Front,
    Back
}