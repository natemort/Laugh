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
		cam.Targets.Add(this.GetNode<Node2D>("player1"));
		cam.Targets.Add(this.GetNode<Node2D>("player2"));

		Rect2 viewport = this.GetViewportRect();
		// cam.LimitLeft = (int) viewport.Position.X;
		GD.Print(cam.LimitLeft);
		// cam.LimitRight = (int) viewport.End.X;
		GD.Print(cam.LimitRight);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
