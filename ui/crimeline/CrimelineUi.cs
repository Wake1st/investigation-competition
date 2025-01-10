using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class CrimelineUi : Panel
{
  const int FONT_SIZE = 24;
  private readonly Color FONT_COLOR = Color.Color8(100, 100, 100, 255);
  private CrimeLine Crimeline { get; set; } = new();

  public override void _Ready()
  {
    TimelineGenerator generator = new();
    Crimeline = generator.Generate();

    //  add location labels
    Vector2 lableOffset = new(16, 64);
    foreach ((Location location, int index) in generator.Locations.WithIndex())
    {
      AddLable(location.Term, lableOffset + new Vector2(0, index * 64));
    }

    //  add time labels
    lableOffset = new(256, 16);
    int startHour = generator.StartTime.Hour - 1;
    int endHour = generator.EndTime.Hour + 1;
    int crimeDuration = endHour - startHour;
    for (int i = 0; i < crimeDuration; i++)
    {
      AddLable($"{startHour + i}:00", lableOffset + new Vector2(i * 64, 0));
    }
  }

  private void AddLable(string text, Vector2 position)
  {
    Label label = new()
    {
      Text = text,
      Position = position,
      LabelSettings = new LabelSettings
      {
        FontSize = FONT_SIZE,
        FontColor = FONT_COLOR
      }
    };
    AddChild(label);
  }
}

public static class Extentions
{
  public static IEnumerable<(T item, int index)> WithIndex<[MustBeVariant] T>(this IEnumerable<T> source)
  {
    return source.Select((item, index) => (item, index));
  }
}
