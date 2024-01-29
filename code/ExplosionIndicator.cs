using Godot;
using System;

public partial class ExplosionIndicator : Node2D
{
	[Export] public float ExplosionDelayMs = 4000;
	[Export] public PackedScene Explosion;

	private float _startTime;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_startTime = Time.GetTicksMsec();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if ((Time.GetTicksMsec() - _startTime) > ExplosionDelayMs)
		{
			var explosion = Explosion.Instantiate<Node2D>();
			explosion.GlobalPosition = GlobalPosition;
			GetParent().CallDeferred("add_child", explosion);
			this.QueueFree();
		}
	}
}
