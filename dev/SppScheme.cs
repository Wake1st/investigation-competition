using Godot;

[Tool]
[GlobalClass]
public partial class SppScheme : Node2D
{
  [Export]
  public Node2D FrontalLeft
  {
    get => frontalLeft;
    set
    {
      Vector2 position = frontalLeft.Position;
      frontalLeft = value;
      frontalLeft.Position = position;
    }
  }
  private Node2D frontalLeft = new Node2D();
  [Export]
  public Node2D FrontalRight
  {
    get => frontalRight;
    set
    {
      Vector2 position = frontalRight.Position;
      frontalRight = value;
      frontalRight.Position = position;
    }
  }
  private Node2D frontalRight = new Node2D();
  [Export]
  public Node2D PictureLeft
  {
    get => pictureLeft;
    set
    {
      GD.Print("picture left");
      if (HasNode("FrontalLeft"))
      {
        pictureLeft = value;
        NotifyPropertyListChanged();

        //  move the frontal node on this side
        Vector2 position = GetNode<Node2D>("FrontalLeft").Position;
        float newX = pictureLeft.Position.X / pictureLeft.Position.Y * position.Y;
        GetNode<Node2D>("FrontalLeft").Position = new Vector2(newX, position.Y);

        //  move the line on this side
        GetNode<Line2D>("LeftLine").SetPointPosition(1, pictureLeft.Position);
      }
    }
  }
  private Node2D pictureLeft = new Node2D();
  [Export]
  public Node2D PictureRight
  {
    get => pictureRight;
    set
    {
      GD.Print("picture right");
      if (HasNode("FrontalRight"))
      {
        pictureRight = value;
        NotifyPropertyListChanged();

        //  move the frontal node on this side
        Vector2 position = GetNode<Node2D>("FrontalRight").Position;
        float newX = pictureRight.Position.X / pictureRight.Position.Y * position.Y;
        GetNode<Node2D>("FrontalRight").Position = new Vector2(newX, position.Y);

        //  move the line on this side
        GetNode<Line2D>("RightLine").SetPointPosition(1, pictureRight.Position);
      }
    }
  }
  private Node2D pictureRight = new Node2D();

  public override void _Ready()
  {
    base._Ready();

    FrontalLeft = GetNode<Node2D>("FrontalLeft");
    PictureLeft = GetNode<Node2D>("PictureLeft");
    FrontalRight = GetNode<Node2D>("FrontalRight");
    PictureRight = GetNode<Node2D>("PictureRight");

    FrontalLeft.ItemRectChanged += OnRectChanged;
    PictureLeft.ItemRectChanged += OnRectChanged;
    FrontalRight.ItemRectChanged += OnRectChanged;
    PictureRight.ItemRectChanged += OnRectChanged;
  }

  public void OnRectChanged()
  {
    GD.Print("Rect changed");
  }
}
