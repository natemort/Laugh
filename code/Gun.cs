using Godot;
using System;
using Laugh.code;

public partial class Gun : Sprite2D, Weapon
{
	[Export]
	public double FireDelayMs = 1000;
	[Export]
	public float BulletSpeed = 1500;

	[Export] public Vector2 BulletOffset = Vector2.Zero;
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
		bullet.GlobalPosition = this.GlobalPosition + BulletOffset;
		bullet.Velocity = Vector2.Right * BulletSpeed;
	}
}
