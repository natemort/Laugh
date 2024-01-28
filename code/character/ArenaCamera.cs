using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using Godot.Collections;

public partial class ArenaCamera : Camera2D
{

	[Export] public float MoveSpeed = 5f;
	[Export] public float ZoomSpeed = 3f;
	[Export] public float MinZoom = 5f; // camera wont zoom closer than this
	[Export] public float MaxZoom = 0.3f; // camera wont zoom farther than this
	[Export] public Array<Node2D> Targets = new();
	[Export] public Vector2 Margin = new Vector2(400f, 250f);

	private HashSet<Node2D> _targets = new HashSet<Node2D>();
	private Vector2 ScreenSize;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = this.GetViewportRect().Size;
		foreach (Node2D n in Targets)
		{
			addTarget(n);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Camera cenetered between 2 targets
		var positions = Vector2.Zero;
		foreach(Node2D n in _targets) positions += n.GlobalPosition;
		positions /= _targets.Count;
		Position = Position.Lerp(positions, MoveSpeed * (float)delta);
		
		
		// Zoom handling
		Rect2 rect = new Rect2(Position, Vector2.One);
		foreach (Node2D n in _targets)
		{
			rect = rect.Expand(n.GlobalPosition);
			// GD.Print("include: " + n.GlobalPosition);
		}
		
		rect = rect.GrowIndividual(Margin.X, Margin.Y, Margin.X, Margin.Y);
		float z;
		// GD.Print("x:" + rect.Size.X + " y*aspect:" + (rect.Size.Y * ScreenSize.Aspect()));
		if (rect.Size.X > rect.Size.Y * ScreenSize.Aspect())
		{
			z = 1 / Math.Clamp(rect.Size.X / ScreenSize.X, MaxZoom, MinZoom);
			// GD.Print("x is bigger");
			// GD.Print(rect.Size.X / ScreenSize.X);
		} else
		{
			z = 1 / Math.Clamp(rect.Size.Y / ScreenSize.Y, MaxZoom, MinZoom);
			// GD.Print("y is bigger ");
			// GD.Print(rect.Size.Y / ScreenSize.Y);
		}
		// GD.Print(z);
		
		Zoom = Zoom.Lerp(Vector2.One * z, ZoomSpeed * (float)delta);
		// GD.Print(Zoom);

	}

	public void addTarget(Node2D node)
	{
		GD.Print("camera added: " + node.Position);
		_targets.Add(node);
	}


	public void removeTarget(Node2D node)
	{
		_targets.Remove(node);
	}
}
