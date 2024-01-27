using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class ArenaCamera : Camera2D
{

	[Export] public float MoveSpeed = 0.5f;
	[Export] public float ZoomSpeed = 0.05f;
	[Export] public float MinZoom = 1.5f;
	[Export] public float MaxZoom = 5f;
	public Vector2 Margin = new Vector2(400f, 400f);

	public HashSet<Node> Targets = new HashSet<Node>();
	public Vector2 ScreenSize;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Targets.Add(this.GetNode("player1")))
		{
			GD.Print("added p1");
		}

		if (Targets.Add(this.GetNode("player2")))
		{
			GD.Print("added p2");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Camera cenetered between 2 targets
		var positions = Vector2.Zero;
		foreach(Node2D n in Targets) positions += n.Position;
		positions /= Targets.Count;
		Position = Position.Lerp(positions, MoveSpeed);
		
		// Zoom handling
		Rect2 rect = new Rect2(Position, Vector2.One);
		foreach (Node2D n in Targets)
		{
			rect.Expand(n.Position);
		}

		rect = rect.GrowIndividual(Margin.X, Margin.Y, Margin.X, Margin.Y);
		float max = Math.Max(rect.Size.X, rect.Size.Y);
		float zoom;
		if (rect.Size.X > rect.Size.Y * ScreenSize.Aspect())
		{
			zoom = Math.Clamp(rect.Size.X / ScreenSize.X, MinZoom, MaxZoom);
		} else
		{
			zoom = Math.Clamp(rect.Size.Y / ScreenSize.Y, MinZoom, MaxZoom);
		}

		Zoom = Zoom.Lerp(Vector2.One * zoom, ZoomSpeed);

	}
}
