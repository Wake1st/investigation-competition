using System.ComponentModel.DataAnnotations;
using Godot;

public partial class TimeRange : Resource
{
    public TimePoint Start { get; set; }
    public TimePoint End { get; set; }
}

public partial class TimePoint : Resource
{
    [Range(1, 12)]
    public int Hour { get; set; }
    [Range(0, 60)]
    public int Minute { get; set; }
    public Meridiem Meridiem { get; set; }
}

public enum Meridiem
{
    AM,
    PM
}