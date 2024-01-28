using Godot;
using System;
using Laugh.code;

public partial class MainMenu : Node2D
{
	// Called when the node enters the scene tree for the first time.
	private TransitionManager transitionManager;
	public override void _Ready()
	{
		transitionManager = this.GetTransitionManager();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _onStartGameButtonPressed()
	{
		
		transitionManager.ReturnToArena();
	}

	// public void _onHowToPlayButtonPressed()
	// {
	// 	
	// }

	public void _onQuitGameButtonPressed()
	{
		GetTree().Quit();
	}
}
