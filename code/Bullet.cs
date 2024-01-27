using Godot;
using System;
using Laugh.code;

public partial class Bullet : Area2D, Killable
{
	public Vector2 Velocity { get; set; } = Vector2.Right;

	private double _spawnTime;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_spawnTime = Time.GetTicksMsec();
	}

	public override void _Process(double delta)
	{
		if (Time.GetTicksMsec() - _spawnTime > 10_000)
		{
			Kill();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Position += Velocity * (float)delta;
	}

	public void OnBodyEntered(Node2D node)
	{
		if (node is Killable k)
		{
			k.Kill();
		}
		Kill();
	}

	public void Kill()
	{
		this.QueueFree();
	}
}
