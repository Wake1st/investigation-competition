using Godot;

public partial class Level : Node
{
  private Player player;

  public override void _Ready()
  {
    base._Ready();

    player = GetNode<Player>("Player");
  }
}
