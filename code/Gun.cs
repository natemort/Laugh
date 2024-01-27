using Godot;
using System;
using Laugh.code;

public partial class Gun : Sprite2D, Weapon
{
	[Export]
	public double FireDelayMs = 1000;
	[Export]
	public float BulletSpeed = 1500;

	private double _lastFired;

	private PackedScene _bulletPrototype;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_bulletPrototype = ResourceLoader.Load<PackedScene>("res://prototype/bullet.tscn");
	}

	public void Fire()
	{
		var bullet = _bulletPrototype.Instantiate<Bullet>();
		this._lastFired = Time.GetTicksMsec();
		this.GetTree().Root.AddChild(bullet);
		var spawnPoint = this.GetChild<Node2D>(0).GlobalPosition;
		bullet.GlobalPosition = spawnPoint;
		var direction = ((spawnPoint - GlobalPosition) with { Y = 0 }).Normalized();
		bullet.Velocity = direction * BulletSpeed;
	}
}
