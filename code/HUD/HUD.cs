using Godot;
using System;
using System.Linq;

public partial class HUD : CanvasLayer
{

	[Signal]
	public delegate void StartGameEventHandler();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void UpdatePlayerHealth(PlayerController2 player)
	{
		if (player.Health < 0) return;
		String h = String.Concat(Enumerable.Repeat("Ha", player.Health));
		if (player.PlayerName.Equals("p1"))
		{
			this.GetNode<Label>("Player1HealthLabel").Text = h;
		} else if (player.PlayerName.Equals("p2"))
		{
			this.GetNode<Label>("Player2HealthLabel").Text = h;
		}
	}

}
