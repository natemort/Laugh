using Godot;
using System;

public partial class joe_scene : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.GetNode("ArenaCamera");
		GD.Print("camera ready");
		ArenaCamera cam = this.GetNode<ArenaCamera>("Camera2D");
		if (cam.Targets.Add(this.GetNode<Node2D>("player1")))
		{
			GD.Print("added p1");
		}

		if (cam.Targets.Add(this.GetNode<Node2D>("player2")))
		{
			GD.Print("added p2");
		}
		GD.Print("wfttfff");

		foreach (var node2D in cam.Targets)
		{
			GD.Print(node2D.Position);
		}
		
		GD.Print(cam.Targets.ToString());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
