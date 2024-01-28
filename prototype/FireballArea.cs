using Godot;
using System;
using Laugh.code;

public partial class FireballArea : Area2D
{
	[Export] public float FireballAcelleration = 5f;
	[Export] public float FireballMaxSpeed = 100000000f;

	public Vector2 Velocity = Vector2.One;
	public Vector2 Acceleration;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Acceleration = Velocity * FireballAcelleration;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		// GD.Print("fireball processing");
		Velocity += Acceleration;
		Velocity.X = Math.Clamp(Velocity.X, 0, FireballMaxSpeed);
		Velocity.Y =  Math.Clamp(Velocity.Y, 0, FireballMaxSpeed);

		Rotation = Velocity.Angle();
		// GD.Print("velocity: " + Velocity);
		// GD.Print("position: " + this.Position);
		Position += Velocity * (float) delta; 
	}
	
	public void OnBodyEntered(Node2D node)
	{
		if (node is Killable k)
		{
			k.Kill();
		}
	}
}
