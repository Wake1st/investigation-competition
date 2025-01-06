using System.Collections.Generic;
using Godot;

public partial class Player : Node2D
{
	const float ROOM_CHANGE_DURATION = 0.26f;

	[Export]
	public RoomNode CurrentRoom { get; set; }

	public Interactable CollidingInteractable { get; set; } = null;

	private Character character;
	private List<ClueNode> clueNodes = new List<ClueNode>();
	private Inventory inventory;
	private Camera2D camera;
	private Tween shiftCamera;

	public override void _Ready()
	{
		base._Ready();
		character = GetNode<Character>("Character");
		character.CurrentRoom = CurrentRoom;
		character.ClueCollected += OnClueCollected;
		character.RoomChanged += OnRoomChanged;

		inventory = GetNode<Inventory>("Inventory");
		camera = GetNode<Camera2D>("Camera2D");
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
		shiftCamera = CreateTween();
		shiftCamera.TweenProperty(camera, "global_position", newRoom.GlobalPosition, ROOM_CHANGE_DURATION);
	}
}
