using Godot;
using System;

public partial class Gun : Sprite2D
{
	public String WeaponAction = "TestShoot";
	[Export]
	public double FireDelayMs = 1000;
	[Export]
	public float BulletSpeed = 1500;
	private double _lastFired;

	private PackedScene _bulletPrototype;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_bulletPrototype = ResourceLoader.Load<PackedScene>("res://bullet.tscn");
	}

    
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed(WeaponAction) && (Time.GetTicksMsec() - _lastFired) > FireDelayMs)
		{
			Fire();
		}
	}

	public void Fire()
	{
		var bullet = _bulletPrototype.Instantiate<Bullet>();
		this._lastFired = Time.GetTicksMsec();
		this.GetTree().Root.AddChild(bullet);
		bullet.GlobalPosition = this.GlobalPosition;
		bullet.Velocity = Vector2.Right * BulletSpeed;
	}
}
