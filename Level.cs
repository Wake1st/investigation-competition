using Godot;

public partial class Level : Node
{
  private Player player;
  private Inventory inventory;

  public override void _Ready()
  {
	  base._Ready();

	  player = GetNode<Player>("Player");
	  player.ClueCollected += OnClueCollected;
	  inventory = GetNode<Inventory>("Inventory");
  }

  private void OnClueCollected(Clue clue)
  {
	  inventory.AddClue(clue);
  }
}
