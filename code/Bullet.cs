using Godot;
using System;
using Laugh.code;

public partial class Bullet : CharacterBody2D, Killable
{
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
		var collision = MoveAndCollide(Velocity * (float)delta);
		if (collision == null) return;
		
		if (collision.GetCollider() is Killable k)
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
