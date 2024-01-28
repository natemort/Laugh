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
	public delegate void DeathSignalEventHandler(PlayerController2 player);

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
	[Export] 
	public String DashControl = "p1-dash";
	[Export] 
	public float DashForce = 5000f;
	private Vector2 _dashDirection = Vector2.Zero;
	private bool _canDash = true;
	private bool _dashing = false;
	private float _dashCooldownMs = 1000f;
	private float _lastDashMs = 0f;

	[Export] public Vector2 WeaponOffset = Vector2.Zero;

	private int _health = 3;
	public int Health
	{
		get
		{
			return _health;
		}
		set
		{
			_health = value;
			this.GetNode<HUD>("../HUD/HUD").UpdatePlayerHealth(this);
		}
	}

	

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private bool canDoubleJump = true;
	private Node2D _weapon;
	private float _initialXScale;
	private bool _isForward = true;
	public bool StopPhysics = false;

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
		if (StopPhysics) return;
		if (Input.IsActionJustPressed(ActionControl) && this._weapon != null)
		{
			this._weapon.Call("Fire");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (StopPhysics) return;
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
		
		// velocity.X = Speed * Input.GetAxis(LeftControl, RightControl);
		if (Input.IsActionPressed(LeftControl) && velocity.X >= Speed * Vector2.Left.X)
		{
			velocity.X = Speed * Vector2.Left.X;
			_dashDirection = Vector2.Left;
		} else if (Input.IsActionPressed(RightControl) && velocity.X <= Speed * Vector2.Right.X)
		{
			velocity.X = Speed * Vector2.Right.X;
			_dashDirection = Vector2.Right;
		}else
		{
			// smooth stop
			velocity.X = velocity.Lerp(Vector2.One, 0.25f).X;
		}
		
		if ((_isForward && velocity.X < 0) || (!_isForward && velocity.X > 0) )
		{
			_isForward = !_isForward;
			this.Scale = this.Scale with
			{
				X = -_initialXScale
			};
		}
		
		velocity = Dash(velocity);
		this.Velocity = velocity;
		MoveAndSlide();
	}

	public Vector2 Dash(Vector2 velocity)
	{
		if (Input.IsActionJustPressed(DashControl) && _canDash)
		{
			// do animation?
			GD.Print("dashing");
			velocity = _dashDirection.Normalized() * DashForce;
			_canDash = false;
			_dashing = true;
			_lastDashMs = Time.GetTicksMsec();
			GD.Print(velocity);
		}
		GD.Print(velocity);
		// dash cooldown reset?
		if (!_canDash && (Time.GetTicksMsec() - _lastDashMs) > _dashCooldownMs)
		{
			GD.Print("can dash again");
			_canDash = true;
		}

		return velocity;
	}

	public void Kill()
	{
		Health--;
		EmitSignal(SignalName.DeathSignal, this);
	}
}

