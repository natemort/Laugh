using Godot;
using System;

public partial class PlayerController2 : CharacterBody2D
{
	[Export] 
	public const float Speed = 300.0f;
	[Export] 
	public const float JumpVelocity = -400.0f;
	[Export] 
	public const float FastFallVelocity = 400f;

	[Export] 
	public const String LeftControl = "p1-left";
	[Export] 
	public const String RightControl = "p1-right";
	[Export] 
	public const String JumpControl = "p1-jump";
	[Export] 
	private const String FastFallControl = "p1-fast-fall";
	[Export] 
	public const String ActionControl = "p1-action";
	

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed(JumpControl) & IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		} else if (Input.IsActionJustPressed(FastFallControl) & !IsOnFloor())
		{
			velocity.Y = FastFallVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		velocity.X = Speed * Input.GetAxis(LeftControl, RightControl);

		this.Velocity = velocity;
		MoveAndSlide();
	}
}

