using Godot;

public partial class NeighbourTile : Node2D
{
	[Signal]
	public delegate void LiveStateChangedEventHandler(bool Alive);

	private bool _Alive = false;

	public bool IsAlive()
	{
		return _Alive;
	}

	public void SetAlive(bool Alive)
	{
		Sprite2D textureSprite = GetNode<Sprite2D>("Sprite2D");

		_Alive = Alive;

		//the texture for the block uses a region to select from basically a sprite sheet of the two block states
		//so move the regionrect right and left to move the "frame window" in the sprite sheet
		if (_Alive)
			textureSprite.RegionRect = new Rect2(new Vector2(13, 15), new Vector2(235, 235));
		else
			textureSprite.RegionRect = new Rect2(new Vector2(267, 15), new Vector2(235, 235));

		EmitSignal(SignalName.LiveStateChanged, new Variant[] { _Alive });
	}

	public void ToggleAlive()
	{
		if (_Alive)
			SetAlive(false);
		else
			SetAlive(true);
	}

	public void OnInput(Node ViewPort, InputEvent Event, int ShapeIdx)
	{
		if (Input.IsMouseButtonPressed(MouseButton.Left))
		{
			ToggleAlive();
		}
	}
}
