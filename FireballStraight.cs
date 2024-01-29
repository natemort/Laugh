using Godot;
using System;
using Laugh.code;

public partial class FireballStraight : Area2D
{
	[Export] public float FireballSpeed = 400f;
	[Export] public float FireballAcceleration = 61.75f;
	[Export] public float FireballMaxSpeed = 1000f;

	public Vector2 Velocity;
	public Vector2 Direction;
	
	public override void _Ready()
	{
		_getDirectionAndSpeed();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Position += Velocity * (float)delta;
	}

	private void _getDirectionAndSpeed()
	{
		GD.Print("direction: " + Direction);
		GD.Print("direction: " + this.GlobalPosition);
		
		Velocity = Direction * FireballSpeed;
		Rotation = Velocity.Angle();
	}
	
	public void OnBodyEntered(Node2D node)
	{
		GD.Print("node entered ball: " + node.Position);
		if (node is Killable k)
		{
			GD.Print("killable");
			k.Kill();
		}
	}

	public void _on_timer_timeout()
	{
		this.QueueFree();
	}
}
