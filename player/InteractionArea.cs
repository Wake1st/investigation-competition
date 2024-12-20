using Godot;

public partial class InteractionArea : CharacterBody2D
{
  [Signal]
  public delegate void RegisterInteractableEventHandler(Interactable interactable);
  [Signal]
  public delegate void UnRegisterInteractableEventHandler(Interactable interactable);

  public void ConnectInteractable(Interactable interactable)
  {
    EmitSignal(SignalName.RegisterInteractable, interactable);
  }

  public void DisconnectInteractable(Interactable interactable)
  {
    EmitSignal(SignalName.UnRegisterInteractable, interactable);
  }
}
