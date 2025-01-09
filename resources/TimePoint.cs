using System.ComponentModel.DataAnnotations;
using Godot;

[GlobalClass]
public partial class TimePoint : Resource
{
  [Export]
  [Range(1, 24)]
  public int Hour { get; set; }
  [Export]
  [Range(0, 60)]
  public int Minute { 
    get => minute; 
    set {
      minute = value % 60;
    }
  }
  private int minute;
}

public static class TimePointExtensions {
  public static int GetTimeInMinutes(this TimePoint time) {
    return time.Hour * 60 + time.Minute;
  }
}