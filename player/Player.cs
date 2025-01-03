using System.Collections.Generic;
using Godot;

public partial class Player : Node2D
{
	[Export]
	public RoomNode CurrentRoom { get; set; }

	private Character character;
	private List<ClueNode> clueNodes = new List<ClueNode>();
	private Inventory inventory;

	public Interactable CollidingInteractable { get; set; } = null;

	public override void _Ready()
	{
		base._Ready();
		character = GetNode<Character>("Character");
		character.CurrentRoom = CurrentRoom;
		character.ClueCollected += OnClueCollected;
		character.RoomChanged += OnRoomChanged;

		inventory = GetNode<Inventory>("Inventory");
	}

	private void OnClueCollected(ClueNode clueNode)
	{
		if (!clueNodes.Contains(clueNode))
		{
			clueNodes.Add(clueNode);
			inventory.AddClue(clueNode);
			GD.Print("Clue collected: ", clueNode.Name);

			//	destroy the interactable
			character.DestroyInteractable();
		}
	}

	private void OnRoomChanged(RoomNode newRoom)
	{
		Tween shiftPlayer = GetTree().CreateTween();
		shiftPlayer.TweenProperty(this, "global_position", newRoom.GlobalPosition, 0.2);
	}
}
