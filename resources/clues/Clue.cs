using System;
using Godot;

[GlobalClass]
public partial class Clue : Resource
{
  [Export]
  public string Term { get; set; }
  [Export]
  public ClueSignificance Significance { get; set; }
  [Export(PropertyHint.MultilineText)]
  public string Description { get; set; }

  public Guid Id { get; set; } = Guid.NewGuid();
  private bool IsFoux { get; set; } = false;
}

public enum ClueSignificance {
  Personal,
  Locale,
  Motive
}