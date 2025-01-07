
using Godot;

[Tool]
[GlobalClass]
public partial class Weapon : Clue
{
  [Export]
  public KillActionType KillType { get; set; }
}