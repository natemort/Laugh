using Godot;
using System;

public partial class ExplosionIndicator : Node2D
{
	[Export] public float ExplosionDelayMs = 4000;

	private AnimatedSprite2D _indicator;
	private AnimatedSprite2D _explosion;
	private KillArea _killZone;
	private float _startTime;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_startTime = Time.GetTicksMsec();
		_indicator = GetNode<AnimatedSprite2D>("Indicator");
		_explosion = GetNode<AnimatedSprite2D>("Explosion");
		_killZone = GetNode<KillArea>("Hitbox");
		_killZone.Disabled = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if ((Time.GetTicksMsec() - _startTime) > ExplosionDelayMs && !_explosion.Visible)
		{
			_indicator.Visible = false;
			_explosion.Visible = true;
			_explosion.Play();
			_killZone.Disabled = false;
		}
	}


	public void OnAnimationFinished()
	{
		GD.Print("Explosion Over");
		this.QueueFree();
	}
}
