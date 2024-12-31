using Godot;

[GlobalClass]
public partial class KillAction : Action {
    [Export]
    public KillActionType Type { get; set; }
    [Export]
    public Person Victim { get; set; }
}

public enum KillActionType {
    Shot,
    Stab,
    Poison,
    Bludgeon,
    Strangle,
}