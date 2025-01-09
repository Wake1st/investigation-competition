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
  public static string Print(this TimeRange range) {
    return $"{range.Start.Hour}:{range.Start.Minute} - {range.End.Hour}:{range.End.Minute}";
  }

  public static bool DoesIntersect(this TimeRange source, TimeRange other) {
    int sourceStart = source.Start.GetTimeInMinutes();
    int sourceEnd = source.End.GetTimeInMinutes();
    int otherStart = other.Start.GetTimeInMinutes();
    int otherEnd = other.End.GetTimeInMinutes();

    return (otherStart < sourceStart && sourceStart < otherEnd)
      || (otherStart < sourceEnd && sourceEnd < otherEnd)
      || (sourceStart < otherStart && otherEnd < sourceEnd)
      || (otherStart < sourceStart && sourceEnd < otherEnd);
  }

  public static TimeRange GenerateIntersectingRange(this TimeRange source)
  {
    Random rand = new();

    int startHour = rand.Next(
      source.End.Hour == 25 ? 25 : source.End.Hour + 1
    );
    if (startHour == source.Start.Hour && source.Start.Minute == 0)
    {
      startHour--;
    }

    int endHour = rand.Next(startHour, 25);
    if (endHour == source.End.Hour && source.End.Minute == 55)
    {
      endHour++;
    }

    int startMinutes = startHour == source.Start.Hour
      ? rand.Next(0, source.Start.Minute)
      : rand.Next(60);
    int endMinutes = endHour == source.End.Hour
      ? rand.Next(source.End.Minute, 60)
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

  public static TimeRange GenerateDisconnectingRange(this TimeRange source, bool before = true)
  {
    Random rand = new();

    int endHour;
    int startHour;
    if (before)
    {
      endHour = rand.Next(source.Start.Hour);
      startHour = rand.Next(endHour);
    }
    else
    {
      startHour = rand.Next(source.End.Hour + 1, 25);
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

    TimePoint end = after.Start;
    TimePoint start = before.End;

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