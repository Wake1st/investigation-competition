using System;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class TimeRange : Resource
{
  [Export]
  public TimePoint Start { get; set; }
  [Export]
  public TimePoint End { get; set; }
}

public static class TimeRangeExtensions
{
  public static TimeRange GenerateIntersectingRange(this TimeRange range)
  {
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

  public static TimeRange GenerateDisconnectingRange(this TimeRange range, bool before = true)
  {
    Random rand = new();

    int endHour;
    int startHour;
    if (before)
    {
      endHour = rand.Next(range.Start.Hour);
      startHour = rand.Next(endHour);
    }
    else
    {
      startHour = rand.Next(range.End.Hour + 1, 25);
      endHour = rand.Next(startHour, 25);
    }

    int startMinutes = rand.Next(5) * 15;
    int endMinutes = rand.Next(5) * 15;
    return new TimeRange
    {
      Start = new TimePoint { Hour = startHour, Minute = startMinutes },
      End = new TimePoint { Hour = endHour, Minute = endMinutes }
    };
  }

  public static TimeRange GenerateInsertingRange(this TimeRange before, TimeRange after)
  {
    Random rand = new();

    int startHour = rand.Next(before.End.Hour, after.Start.Hour);
    int endHour = rand.Next(startHour, after.Start.Hour);
    int startMinutes = rand.Next(60);
    int endMinutes = endHour == startHour
      ? rand.Next(startMinutes, 60)
      : rand.Next(60);

    return new TimeRange
    {
      Start = new TimePoint { Hour = startHour, Minute = startMinutes },
      End = new TimePoint { Hour = endHour, Minute = endMinutes }
    };
  }

  public static Array<TimeRange> GenerateInsertingRanges(this TimeRange before, TimeRange after)
  {
    Random rand = new();
    Array<TimeRange> ranges = new();

    TimePoint start = after.Start;
    TimePoint end = before.End;
    int insertionMinutes = (end.Hour - start.Hour) * 60 + end.Minute - start.Minute;
    int nodeCount = rand.Next(insertionMinutes / 30);
    for (int i = 0; i < nodeCount; i++)
    {
      //  set up an relative span
      int minuteSpan = i * rand.Next(3) * 15;
      minuteSpan = minuteSpan > insertionMinutes ? insertionMinutes : minuteSpan;
      insertionMinutes -= minuteSpan;

      //  add the new range
      int adjustedMinutes = start.Minute + minuteSpan;
      ranges.Add(new TimeRange
      {
        Start = new TimePoint
        {
          Hour = start.Hour,
          Minute = start.Minute
        },
        End = new TimePoint
        {
          Hour = start.Hour + adjustedMinutes / 60,
          Minute = start.Minute + adjustedMinutes
        }
      });

      //  if the minutes are done, then exit
      if (insertionMinutes == 0)
      {
        break;
      }
    }

    return ranges;
  }
}