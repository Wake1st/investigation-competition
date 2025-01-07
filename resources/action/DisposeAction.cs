using Godot;

[GlobalClass]
public partial class DisposeAction : Action {
    [Export]
    public DisposeActionType Type { get; set; }
}

public enum DisposeActionType {
    Hide,
    Bury,
    Burn,
    Clean,
    Plant,
}