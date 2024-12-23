using System.Linq;
using Godot;

public partial class Inventory : Control
{

  private HBoxContainer EvidenceList;

  public override void _Ready()
  {
    base._Ready();
    EvidenceList = GetNode<HBoxContainer>("%EvidenceList");

    foreach (EvidenceSlot slot in EvidenceList.GetChildren().Cast<EvidenceSlot>())
    {
      // slot.SlotResized += OnEvidenceSlotResized;
    }
  }

  private void OnEvidenceSlotResized()
  {
    GD.Print("event caught");
  }
}
