using Godot;

public partial class Inventory : Control
{
  private PackedScene slotScene = ResourceLoader.Load<PackedScene>("res://ui/evidence_slot.tscn");

  private HBoxContainer EvidenceList;

  public override void _Ready()
  {
    base._Ready();
    EvidenceList = GetNode<HBoxContainer>("%EvidenceList");
  }

  public void AddClue(ClueNode clueNode)
  {
    EvidenceSlot slot = slotScene.Instantiate<EvidenceSlot>();
    EvidenceList.AddChild(slot);
    slot.Setup(clueNode);
  }
}
