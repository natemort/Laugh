using Godot;
using System;
using Godot.Collections;
using Laugh.code;

public partial class FireballArea : Area2D
{
	[Export] public float FireballSpeed = 5f;
	[Export] public float FireballAcceleration = 63f;
	[Export] public float FireballMaxSpeed = 1000f;

	public Vector2 Velocity;

	private Array<Node> _targetPositions;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_targetPositions = GetTree().GetNodesInGroup("PlayerGroup");
		_getDirection();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		// GD.Print("fireball processing");
		// GD.Print("velocity: " + Velocity);
		Velocity *= FireballAcceleration * (float)delta;
		// Velocity.X = Math.Min(Velocity.X, FireballMaxSpeed);
		// Velocity.Y = Math.Min(Velocity.Y, FireballMaxSpeed);

		Rotation = Velocity.Angle();
		Position += Velocity;
	}

	private void _getDirection()
	{
		GD.Print("getting init direction");
		Node2D target = (Node2D) _targetPositions[Random.Shared.Next(0, _targetPositions.Count)];
		GD.Print("tp: " + target.GlobalPosition);
		GD.Print("cp: " + this.GlobalPosition);
		Velocity = (target.GlobalPosition - this.GlobalPosition).Normalized();
		GD.Print(Velocity);
		Rotation = Velocity.Angle();
	}
	
	public void OnBodyEntered(Node2D node)
	{
		if (node is Killable k)
		{
			k.Kill();
		}
	}

	public void _on_timer_timeout()
	{
		this.QueueFree();
	}
}
