using System;
using Godot;

[GlobalClass]
public partial class TimeRange : Resource
{
    [Export]
    public TimePoint Start { get; set; }
    [Export]
    public TimePoint End { get; set; }
}

public static class TimeRangeExtensions {
  public static TimeRange GenerateIntersectingRange(this TimeRange range) {
    Random rand = new();
      
    int startHour = rand.Next(
      range.End.Hour == 25 ? 25 : range.End.Hour + 1
    );
    if (startHour == range.Start.Hour && range.Start.Minute == 0)
    {
      startHour--;
    }

    int endHour = rand.Next(startHour, 25);
    if (endHour == range.End.Hour && range.End.Minute == 55)
    {
      endHour++;
    }

    int startMinutes = startHour == range.Start.Hour
      ? rand.Next(0, range.Start.Minute)
      : rand.Next(60);
    int endMinutes = endHour == range.End.Hour
      ? rand.Next(range.End.Minute, 60)
      : rand.Next(60);

    return new TimeRange()
    {
      Start = new TimePoint()
      {
        Hour = startHour,
        Minute = startMinutes
      },
      End = new TimePoint()
      {
        Hour = endHour,
        Minute = endMinutes
      }
    };
  }

  public static TimeRange GenerateDisconnectingRange(this TimeRange range, bool before = true) {
    Random rand = new();
    
    int endHour;
    int startHour;
    if (before) {
      endHour = rand.Next(range.Start.Hour);
      startHour = rand.Next(endHour);
    } else {
      startHour = rand.Next(range.End.Hour + 1, 25);
      endHour = rand.Next(startHour, 25);
    }
    
    int startMinutes = rand.Next(5) * 15;
    int endMinutes = rand.Next(5) * 15;
    return new TimeRange {
      Start = new TimePoint { Hour = startHour, Minute = startMinutes },
      End = new TimePoint { Hour = endHour, Minute = endMinutes }
    };
  }
}