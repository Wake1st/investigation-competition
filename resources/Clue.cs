using System;
using Godot;

public partial class Clue : Resource
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string Term { get; set; }
  public string Description { get; set; }
  private bool IsFoux { get; set; } = false;
}