using Godot;
using System;
using Laugh.code;

public partial class KillArea : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	public void OnBodyEntered(Node2D node)
	{
		if (node is Killable k)
		{
			k.Kill();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
