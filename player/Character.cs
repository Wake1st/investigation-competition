using Godot;
using System;

public partial class Character : Node2D
{
  const float COORD3D_CLAMP_X = .54f;
  const float COORD3D_CLAMP_Z = .18f;

  const float HEIGHT_TO_FOCAL = -642f;
  const float MAX_FLOOR_HALF_WIDTH = 512;
  const float MID_FLOOR_HALF_WIDTH = 448f;

  const float PLANAR_SPEED = .3f;
  const float DEPTH_SPEED = .2f;

  public RoomNode CurrentRoom { get; set; }

  [Signal]
  public delegate void ClueCollectedEventHandler(ClueNode clueNode);
  [Signal]
  public delegate void RoomChangedEventHandler(RoomNode newRoom);

  private Vector3 coord3D = new(0f, 0f, 0f);
  private Sprite2D sprite;
  private InteractionArea interactionArea;

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

  public void DestroyInteractable()
  {
    //	destroy the interactable
    ((ClueInteractable)CollidingInteractable).Destroy();
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
    Vector3 previousCoord3D = coord3D;
    coord3D += new Vector3(movement.X * PLANAR_SPEED, 0.0f, -movement.Y * DEPTH_SPEED) * delta;

    //	set position
    float slantHeight = Position.Y - HEIGHT_TO_FOCAL;
    float slantWidth = Mathf.Lerp(0, MAX_FLOOR_HALF_WIDTH, slantHeight);
    float flatHorizontalMultiplier = slantWidth / MID_FLOOR_HALF_WIDTH;
    float slatRatio = Mathf.Abs(slantWidth / slantHeight);
    float flatVerticalMultiplier = coord3D.Z * slatRatio;

    //  reset the 3D coords if there is clamping
    Vector2 updatedPosition = new(
      coord3D.X * flatHorizontalMultiplier,
      flatVerticalMultiplier
    );
    Vector2 clampedPosition = CurrentRoom.ClampWithinBoundaries(updatedPosition);
    if (updatedPosition != clampedPosition)
    {
      coord3D = previousCoord3D;
    }

    //  finally, set the position
    Position = clampedPosition;
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
      switch (CollidingInteractable.GetType().ToString())
      {
        case "ClueInteractable":
          ClueInteractable clueInteractable = (ClueInteractable)CollidingInteractable;

          //	pick up the clue
          ClueNode clueNode = clueInteractable.ClueNode;
          EmitSignal(SignalName.ClueCollected, clueNode);

          break;
        case "DoorInteractable":
          DoorInteractable doorInteractable = (DoorInteractable)CollidingInteractable;

          //	change rooms
          CurrentRoom = doorInteractable.ConnectingDoor.CurrentRoom;
          EmitSignal(SignalName.RoomChanged, CurrentRoom);
          Position = CurrentRoom.Position + doorInteractable.ConnectingDoor.Position;

          break;
        default:
          throw new Exception($"Interactable type not recognised: {CollidingInteractable}");
      }

      CollidingInteractable = null;
    }
  }
}
