using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class ArenaCamera : Camera2D
{

	[Export] public float MoveSpeed = 0.5f;
	[Export] public float ZoomSpeed = 0.05f;
	[Export] public float MinZoom = 5f; // camera wont zoom closer than this
	[Export] public float MaxZoom = 0.5f; // camera wont zoom farther than this
	public Vector2 Margin = new Vector2(400f, 250f);

	public HashSet<Node2D> Targets = new HashSet<Node2D>();
	public Vector2 ScreenSize;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = this.GetViewportRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Camera cenetered between 2 targets
		var positions = Vector2.Zero;
		foreach(Node2D n in Targets) positions += n.Position;
		positions /= Targets.Count;
		Position = Position.Lerp(positions, MoveSpeed * (float)delta);
		
		
		// Zoom handling
		Rect2 rect = new Rect2(Position, Vector2.One);
		foreach (Node2D n in Targets)
		{
			rect = rect.Expand(n.Position);
			GD.Print("include: " + n.GlobalPosition);
		}
		
		rect = rect.GrowIndividual(Margin.X, Margin.Y, Margin.X, Margin.Y);
		float z;
		GD.Print("x:" + rect.Size.X + " y*aspect:" + (rect.Size.Y * ScreenSize.Aspect()));
		if (rect.Size.X > rect.Size.Y * ScreenSize.Aspect())
		{
			z = 1 / Math.Clamp(rect.Size.X / ScreenSize.X, MaxZoom, MinZoom);
			GD.Print("x is bigger");
			GD.Print(rect.Size.X / ScreenSize.X);
		} else
		{
			z = 1 / Math.Clamp(rect.Size.Y / ScreenSize.Y, MaxZoom, MinZoom);
			GD.Print("y is bigger ");
			GD.Print(rect.Size.Y / ScreenSize.Y);
		}
		// GD.Print(z);
		
		Zoom = Zoom.Lerp(Vector2.One * z, ZoomSpeed * (float)delta);
		GD.Print(Zoom);

	}
}
