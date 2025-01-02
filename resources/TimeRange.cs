using Godot;

[GlobalClass]
public partial class TimeRange : Resource
{
    [Export]
    public TimePoint Start { get; set; }
    [Export]
    public TimePoint End { get; set; }
}
