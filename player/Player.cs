using System.Collections.Generic;
using Godot;

public partial class Player : Node2D
{
	const float COORD3D_CLAMP_X = .54f;
	const float COORD3D_CLAMP_Z = .18f;

	const float HEIGHT_TO_FOCAL = -642f;
	const float MAX_FLOOR_HALF_WIDTH = 512;
	const float MID_FLOOR_HALF_WIDTH = 448f;

	const float PLANAR_SPEED = .3f;
	const float DEPTH_SPEED = .2f;

	[Signal]
	public delegate void ClueCollectedEventHandler(ClueNode clueNode);

	private Vector3 coord3D = new(0f, 0f, 0f);
	private Sprite2D sprite;
	private InteractionArea interactionArea;
	private List<ClueNode> ClueNodes = new();

	public Interactable CollidingInteractable { get; set; } = null;

	public override void _Ready()
	{
		base._Ready();
		sprite = GetNode<Sprite2D>("Image");

		interactionArea = GetNode<InteractionArea>("InteractionArea");
		interactionArea.RegisterInteractable += OnRegisterInteractable;
		interactionArea.UnRegisterInteractable += OnUnregisterInteractable;
	}

	public override void _PhysicsProcess(double delta)
	{
		//	player movement
		Vector2 movement = MovementInputs();
		Move(movement, (float)delta);
		Animate(movement);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept"))
			ProcessAction();
	}

	private void OnRegisterInteractable(Interactable interactable)
	{
		CollidingInteractable = interactable;
	}

	private void OnUnregisterInteractable(Interactable interactable)
	{
		if (CollidingInteractable == interactable)
			CollidingInteractable = null;
	}

	private static Vector2 MovementInputs()
	{
		return new()
		{
			X = (Input.IsActionPressed("ui_left"), Input.IsActionPressed("ui_right")) switch
			{
				(false, false) => 0.0f,
				(true, false) => -1.0f,
				(false, true) => 1.0f,
				(true, true) => 0.0f,
			},
			Y = (Input.IsActionPressed("ui_down"), Input.IsActionPressed("ui_up")) switch
			{
				(false, false) => 0.0f,
				(true, false) => -1.0f,
				(false, true) => 1.0f,
				(true, true) => 0.0f,
			}
		};
	}

	private void Move(Vector2 movement, float delta)
	{
		//	update coordinates
		coord3D += new Vector3(movement.X * PLANAR_SPEED, 0.0f, -movement.Y * DEPTH_SPEED) * delta;
		coord3D.X = Mathf.Clamp(coord3D.X, -COORD3D_CLAMP_X, COORD3D_CLAMP_X);
		coord3D.Z = Mathf.Clamp(coord3D.Z, -COORD3D_CLAMP_Z, COORD3D_CLAMP_Z);

		//	set position
		float slantHeight = Position.Y - HEIGHT_TO_FOCAL;
		float slantWidth = Mathf.Lerp(0, MAX_FLOOR_HALF_WIDTH, slantHeight);
		float flatHorizontalMultiplier = slantWidth / MID_FLOOR_HALF_WIDTH;
		float slatRatio = Mathf.Abs(slantWidth / slantHeight);

		float flatVerticalMultiplier = coord3D.Z * slatRatio;
		Position = new(
			coord3D.X * flatHorizontalMultiplier,
			flatVerticalMultiplier
		);
	}

	private void Animate(Vector2 movement)
	{
		//	animate sprite
		if (sprite.FlipH && movement.X < 0f)
			sprite.FlipH = false;
		else if (!sprite.FlipH && movement.X > 0f)
			sprite.FlipH = true;
	}

	private void ProcessAction()
	{
		if (CollidingInteractable != null)
		{
			ClueNode clueNode = CollidingInteractable.ClueNode;
			if (!ClueNodes.Contains(clueNode))
			{
				ClueNodes.Add(clueNode);
				EmitSignal(SignalName.ClueCollected, clueNode);
				GD.Print("Clue collected: ", clueNode.Name);

				//	destroy the interactable
				CollidingInteractable.Destroy();
				CollidingInteractable = null;
			}
		}
	}
}
