using Godot;

[Tool]
[GlobalClass]
public partial class SppScheme : Node2D
{
  [Export]
  public bool VerticalSymetry { get; set; }
  [Export]
  public AnchorPoint FrontalLeft { get; set; }
  [Export]
  public AnchorPoint FrontalRight { get; set; }
  [Export]
  public AnchorPoint PictureLeft { get; set; }
  [Export]
  public AnchorPoint PictureRight { get; set; }

  [Export]
  public Line2D LeftSide { get; set; }
  [Export]
  public Line2D RightSide { get; set; }
  [Export]
  public Line2D FrontalLine { get; set; }
  [Export]
  public Line2D PictureLine { get; set; }

  public override void _Process(double delta)
  {
    base._Process(delta);

    if (Engine.IsEditorHint()) {
      if (PictureLeft.CheckDelta() != Vector2.Zero) {
        GD.Print("picture left moved");

        //  move the frontal node on this side
        RepositionPlanarPartner(PictureLeft, PictureRight);
        RepositionSidePartner(PictureLeft, FrontalLeft);
        RepositionSidePartner(PictureRight, FrontalRight);

        //  move the connecting lines
        RepositionLines();
      } else if (PictureRight.CheckDelta() != Vector2.Zero) {
        GD.Print("picture right moved");

        //  move the frontal node on this side
        RepositionPlanarPartner(PictureRight, PictureLeft);
        RepositionSidePartner(PictureRight, FrontalRight);
        RepositionSidePartner(PictureLeft, FrontalLeft);

        //  move the connecting lines
        RepositionLines();
      } else if (FrontalLeft.CheckDelta() != Vector2.Zero) {
        GD.Print("frontal left moved");

        //  move the picture node on this side
        RepositionPlanarPartner(FrontalLeft, FrontalRight);
        RepositionSidePartner(FrontalLeft, PictureLeft);
        RepositionSidePartner(FrontalRight, PictureRight);

        //  move the connecting lines
        RepositionLines();
      } else if (FrontalRight.CheckDelta() != Vector2.Zero) {
        GD.Print("frontal right moved");

        //  move the frontal node on this side
        RepositionPlanarPartner(FrontalRight, FrontalLeft);
        RepositionSidePartner(FrontalRight, PictureRight);
        RepositionSidePartner(FrontalLeft, PictureLeft);

        //  move the connecting lines
        RepositionLines();
      }
    }
  }

  private void RepositionSidePartner(AnchorPoint node, AnchorPoint partner) {
    Vector2 position = partner.Position;
    float newX = node.Position.X / node.Position.Y * position.Y;
    partner.Reposition(new Vector2(newX, position.Y));
  }

  private void RepositionPlanarPartner(AnchorPoint node, AnchorPoint partner) {
    partner.Reposition(new Vector2(-node.Position.X, node.Position.Y));
  }

  private void ReevaluatePosition(AnchorPoint pictureNode, AnchorPoint frontalNode, Line2D sideLine, bool leftSide) {
    //  get the side index
    int sideIndex = leftSide ? 0 : 1;

    //  move the connecting lines
    sideLine.SetPointPosition(1, pictureNode.Position);
    FrontalLine.SetPointPosition(sideIndex, frontalNode.Position);
    PictureLine.SetPointPosition(sideIndex, pictureNode.Position);
  }

  private void RepositionLines() {
    LeftSide.SetPointPosition(1, PictureLeft.Position);
    RightSide.SetPointPosition(1, PictureRight.Position);
    FrontalLine.SetPointPosition(0, FrontalLeft.Position);
    PictureLine.SetPointPosition(0, PictureLeft.Position);
    FrontalLine.SetPointPosition(1, FrontalRight.Position);
    PictureLine.SetPointPosition(1, PictureRight.Position);
  }
}
