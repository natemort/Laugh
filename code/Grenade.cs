using Godot;
using System;
using Laugh.code;

public partial class Grenade : RigidBody2D, Killable
{
	[Export] public PackedScene Explosion;
	[Export] public float LifetimeMs = 5_000;

	private float _spawnTime;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_spawnTime = Time.GetTicksMsec();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if ((Time.GetTicksMsec() - _spawnTime) > LifetimeMs)
		{
			Kill();
		}
	}

	public void OnBodyEntered(Node body)
	{
		GD.Print("happened?");
		if (body is Killable)
		{
			// Trigger explosion which would actually kill them
			Kill();
		}
	}
	
	public void Launch(Vector2 direction)
	{
		this.ApplyImpulse(direction);
	}

	public void Kill()
	{
		var explosion = Explosion.Instantiate<Node2D>();
		explosion.GlobalPosition = GlobalPosition;
		GetParent().CallDeferred("add_child", explosion);
		this.QueueFree();
	}
}
