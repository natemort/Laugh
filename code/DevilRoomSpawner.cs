using Godot;
using System;
using Laugh.code;

public partial class DevilRoomSpawner : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var player = this.GetTransitionManager().GetWinner();
		this.GetParent().AddChild(player);
		player.GlobalPosition = this.GlobalPosition;
	}

}
