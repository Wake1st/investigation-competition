using Godot;

public partial class Motive : GodotObject {
    public string Id { get; set; }
    public MotiveStrength Strength { get; set; }
    public MotiveDesire Desire { get; set; }
}

public enum MotiveStrength {
    Weak,
    Fair,
    Strong,
}

public enum MotiveDesire {
    Money,
    Power,
    Love,
    Revenge,
}