using System.ComponentModel.DataAnnotations;
using Godot;

[GlobalClass]
public partial class TimePoint : Resource
{
  [Export]
  [Range(1, 12)]
  public int Hour { get; set; }
  [Export]
  [Range(0, 60)]
  public int Minute { get; set; }
  [Export]
  public Meridiem Meridiem { get; set; }
}

public enum Meridiem
{
  AM,
  PM
}