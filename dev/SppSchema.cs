using Godot;

[Tool]
public partial class SppSchema : Node2D
{
  private AnchorPoint FrontalLeft { get; set; }
  private AnchorPoint FrontalRight { get; set; }
  private AnchorPoint PictureLeft { get; set; }
  private AnchorPoint PictureRight { get; set; }

  private Line2D LeftLine { get; set; }
  private Line2D RightLine { get; set; }
  private Line2D FrontalLine { get; set; }
  private Line2D PictureLine { get; set; }

  public Vector2 ClampWithinBoundaries(Vector2 position)
  {
    Vector2 localPosition = position - GlobalPosition;
    float ratio = (localPosition.Y - FrontalRight.Position.Y) / (PictureRight.Position.Y - FrontalRight.Position.Y);
    float cappedX = Mathf.Lerp(0f, PictureRight.Position.X - FrontalRight.Position.X, ratio);
    return new Vector2(
      Mathf.Clamp(position.X, FrontalLeft.GlobalPosition.X - cappedX, FrontalRight.GlobalPosition.X + cappedX),
      Mathf.Clamp(position.Y, FrontalRight.GlobalPosition.Y, PictureRight.GlobalPosition.Y)
    );
  }

  public override void _Ready()
  {
    base._Ready();
    CaptureNodes();
  }

  public override void _Process(double delta)
  {
    base._Process(delta);

    if (Engine.IsEditorHint())
    {
      //  first, ensure the nodes are captured
      CaptureNodes();

      if (PictureLeft.CheckDelta() != Vector2.Zero)
      {
        GD.Print("picture left moved");

        //  move the frontal node on this side
        RepositionPlanarPartner(PictureLeft, PictureRight);
        RepositionSidePartner(PictureLeft, FrontalLeft);
        RepositionSidePartner(PictureRight, FrontalRight);

        //  move the connecting lines
        RepositionLines();
      }
      else if (PictureRight.CheckDelta() != Vector2.Zero)
      {
        GD.Print("picture right moved");

        //  move the frontal node on this side
        RepositionPlanarPartner(PictureRight, PictureLeft);
        RepositionSidePartner(PictureRight, FrontalRight);
        RepositionSidePartner(PictureLeft, FrontalLeft);

        //  move the connecting lines
        RepositionLines();
      }
      else if (FrontalLeft.CheckDelta() != Vector2.Zero)
      {
        GD.Print("frontal left moved");

        //  move the picture node on this side
        RepositionPlanarPartner(FrontalLeft, FrontalRight);
        RepositionSidePartner(FrontalLeft, PictureLeft);
        RepositionSidePartner(FrontalRight, PictureRight);

        //  move the connecting lines
        RepositionLines();
      }
      else if (FrontalRight.CheckDelta() != Vector2.Zero)
      {
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

  private void CaptureNodes()
  {
    FrontalLeft = GetNode<AnchorPoint>("FrontalLeft");
    FrontalRight = GetNode<AnchorPoint>("FrontalRight");
    PictureLeft = GetNode<AnchorPoint>("PictureLeft");
    PictureRight = GetNode<AnchorPoint>("PictureRight");
    LeftLine = GetNode<Line2D>("LeftLine");
    RightLine = GetNode<Line2D>("RightLine");
    FrontalLine = GetNode<Line2D>("FrontalLine");
    PictureLine = GetNode<Line2D>("PictureLine");
  }

  private void RepositionPlanarPartner(AnchorPoint node, AnchorPoint partner)
  {
    partner.Reposition(new Vector2(-node.Position.X, node.Position.Y));
  }

  private void RepositionSidePartner(AnchorPoint node, AnchorPoint partner)
  {
    Vector2 position = partner.Position;
    float newX = node.Position.X / node.Position.Y * position.Y;
    partner.Reposition(new Vector2(newX, position.Y));
  }

  private void RepositionLines()
  {
    LeftLine.SetPointPosition(1, PictureLeft.Position);
    RightLine.SetPointPosition(1, PictureRight.Position);
    FrontalLine.SetPointPosition(0, FrontalLeft.Position);
    PictureLine.SetPointPosition(0, PictureLeft.Position);
    FrontalLine.SetPointPosition(1, FrontalRight.Position);
    PictureLine.SetPointPosition(1, PictureRight.Position);
  }
}
