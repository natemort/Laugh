using Godot;
using System;
using System.Numerics;
using Laugh.code;
using Vector2 = Godot.Vector2;

public partial class PlayerController2 : CharacterBody2D, Killable
{
	[Export] public float Speed = 300.0f;
	[Export] public float JumpVelocity = -600f;
	[Export] public float FastFallVelocity = 400f;

	[Signal]
	public delegate void DeathSignalEventHandler();

	public int Health = 3;
	[Export] 
	public string PlayerName = "p1";

	[Export] 
	public String LeftControl = "p1-left";
	[Export] 
	public String RightControl = "p1-right";
	[Export] 
	public String JumpControl = "p1-jump";
	[Export] 
	private String FastFallControl = "p1-fast-fall";
	[Export] 
	public String ActionControl = "p1-action";
	[Export] 
	public String DeathControl = "p1-death";

	[Export] public Vector2 WeaponOffset = Vector2.Zero;
	

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private bool canDoubleJump = true;
	private Node2D _weapon;
	private float _initialXScale;
	private bool _isForward = true;

	public override void _Ready()
	{

		SetWeapon(ResourceLoader.Load<PackedScene>("res://prototype/revolver.tscn").Instantiate<Gun>());
		_initialXScale = this.Scale.X;
	}

	public void SetWeapon(Node2D weapon) 
	{
		this._weapon?.QueueFree();
		weapon.Position = WeaponOffset;
		this.AddChild(weapon);
		this._weapon = weapon;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed(ActionControl) && this._weapon != null)
		{
			this._weapon.Call("Fire");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		
		// Handle death conditions
		if (Input.IsActionJustPressed(DeathControl))
		{
			Kill();
		}

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;
		if (IsOnFloor())
			canDoubleJump = true;

		// Handle Jump.
		if (Input.IsActionJustPressed(JumpControl))
		{
			if (IsOnFloor())
			{
				velocity.Y = JumpVelocity;
			} 
			else if (canDoubleJump)
			{
				canDoubleJump = false;
				velocity.Y = JumpVelocity;
			}
			
		} else if (Input.IsActionJustPressed(FastFallControl) && !IsOnFloor()) // Fast Fall
		{
			if (velocity.Y < 0) velocity = Vector2.Zero;
			velocity.Y += FastFallVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		velocity.X = Speed * Input.GetAxis(LeftControl, RightControl);

		if ((_isForward && velocity.X < 0) || (!_isForward && velocity.X > 0) )
		{
			_isForward = !_isForward;
			this.Scale = this.Scale with
			{
				X = -_initialXScale
			};
		}

		this.Velocity = velocity;
		MoveAndSlide();
	}

	public void Kill()
	{
		HUD hud = this.GetNode<HUD>("../HUD/HUD");
		Health--;
		
		hud.UpdatePlayerHealth(this);
	}
}

