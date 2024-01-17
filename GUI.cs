using Godot;
using System;

public partial class GUI : Node2D
{
	[Signal]
	public delegate void PlayButtonPressedEventHandler(int PlaySpeed);

	[Signal]
	public delegate void ResetButtonPressedEventHandler();

	double _PlaySpeed = 0.01;

	public void OnTextureButtonPressed()
	{
		EmitSignal(SignalName.PlayButtonPressed, new Variant[] { _PlaySpeed });
	}

	public void OnTextureButton2Pressed()
	{
		EmitSignal(SignalName.ResetButtonPressed);
	}

	public void SetPlayingState(bool IsPlaying)
	{
		if (IsPlaying)
		{
			var PauseButtonNormal = GD.Load<Texture2D>("res://img/PauseButtonNormal.png");
			var PauseButtonPressed = GD.Load<Texture2D>("res://img/PauseButtonPressed.png");

			TextureButton ppButton = GetNode<TextureButton>("TextureButton");
			ppButton.TextureNormal = PauseButtonNormal;
			ppButton.TexturePressed = PauseButtonPressed;

			GetNode<TextEdit>("TextEdit").Editable = false;
			GetNode<TextureButton>("TextureButton2").Disabled = true;
		}
		else
		{
			var PlayButtonNormal = (Texture2D)GD.Load("res://img/PlayButtonNormal.png");
			var PlayButtonPressed = (Texture2D)GD.Load("res://img/PlayButtonPressed.png");
			TextureButton ppButton = GetNode<TextureButton>("TextureButton");
			ppButton.TextureNormal = PlayButtonNormal;
			ppButton.TexturePressed = PlayButtonPressed;

			GetNode<TextEdit>("TextEdit").Editable = true;
			GetNode<TextureButton>("TextureButton2").Disabled = false;
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
