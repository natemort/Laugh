using Godot;
using System;
using Laugh.code;

public partial class Gun : Sprite2D, Weapon
{
	[Export]
	public double FireDelayMs = 1000;

	[Export] public int FireAngleDegrees = 0;
	[Export]
	public float BulletSpeed = 1500;

	[Export] public PackedScene BulletType;
	[Export] public Node2D FirePoint;

	private double _lastFired;

	public void Fire()
	{
		if ((Time.GetTicksMsec() - _lastFired) > FireDelayMs)
		{
			this._lastFired = Time.GetTicksMsec();
			var bullet = BulletType.Instantiate<Node2D>();
			this.GetTree().Root.AddChild(bullet);
			var spawnPoint = FirePoint.GlobalPosition;
			bullet.GlobalPosition = spawnPoint;
			var direction = ((spawnPoint - GlobalPosition) with { Y = 0 }).Normalized();
			if (direction.X < 0)
			{
				bullet.Scale = bullet.Scale with { X = -1 };
				direction = direction.Rotated(Mathf.DegToRad(-FireAngleDegrees));
			}
			else
			{
				direction = direction.Rotated(Mathf.DegToRad(FireAngleDegrees));
			}

			bullet.Call("Launch", direction * BulletSpeed);
			//bullet.Velocity = direction * BulletSpeed;
		}
	}
}
