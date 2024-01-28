using Godot;
using System;
using System.Numerics;
using Laugh.code;
using Vector2 = Godot.Vector2;

public partial class PlayerController2 : CharacterBody2D, Killable
{
	[Export] public float Speed = 500f;
	[Export] public float JumpVelocity = -900f;
	[Export] public float FastFallVelocity = 400f;
	private float frames = 60f;

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
	public float DashForce = 7500f;
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
		Speed *= frames;
		JumpVelocity *= frames;
		DashForce *= frames;
		FastFallVelocity *= frames;
		
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
		GD.Print(this.PlayerName + " new iteration vel: " + velocity);
		GD.Print("delta = " + delta);
		
		// Handle death conditions
		if (Input.IsActionJustPressed(DeathControl))
		{
			Kill();
		}

		velocity = Gravity(velocity, delta);
		velocity = JumpAndFastFall(velocity, delta);
		
		velocity = DirectionalMovement(velocity, delta);
		velocity = Dash(velocity, delta);
		
		
		this.Velocity = velocity;
		GD.Print("global velocity is: " + Velocity);
		MoveAndSlide();
	}

	private Vector2 Gravity(Vector2 velocity, double delta)
	{
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta * 1.5f;
		if (IsOnFloor())
			canDoubleJump = true;
		return velocity;
	}
	private Vector2 DirectionalMovement(Vector2 velocity, double delta)
	{
		GD.Print("before movement: " + velocity.X);
		if (Input.IsActionPressed(LeftControl) && ! (velocity.X < Speed * Vector2.Left.X * delta))
		{
			GD.Print("left override");
			velocity.X = Speed * Vector2.Left.X * (float)delta;
			_dashDirection = Vector2.Left;
		} else if (Input.IsActionPressed(RightControl) && !(velocity.X > Speed * Vector2.Right.X * delta))
		{
			GD.Print("right override");
			velocity.X = Speed * Vector2.Right.X * (float)delta;
			_dashDirection = Vector2.Right;
		}else
		{
			// smooth stopa
			// GD.Print("lerp override");
			// GD.Print("(Mathf.Lerp(Velocity.X, 0, 0.5f * (float)delta)) = " + (Mathf.Lerp(velocity.X, 0, 0.25f)));

			velocity.X = Mathf.Lerp(velocity.X, 0, .25f);
			// velocity.X =  velocity.Lerp(Vector2.One * _dashDirection *(float)delta, 0.1f).X * (float)delta;	
			// velocity.X = (float) (Mathf.Lerp(velocity.X, Vector2.One.X * Input.GetAxis(LeftControl, RightControl), 0.1) * delta);
		}
		
		
		
		if ((_isForward && velocity.X < 0) || (!_isForward && velocity.X > 0) )
		{
			_isForward = !_isForward;
			this.Scale = this.Scale with
			{
				X = -_initialXScale
			};
		}
		GD.Print("after movement: " + velocity.X);
		return velocity;
	}
	public Vector2 JumpAndFastFall(Vector2 velocity, double delta)
	{
		if (Input.IsActionJustPressed(JumpControl))
		{
			if (IsOnFloor())
			{
				velocity.Y = JumpVelocity * (float)delta;
			} 
			else if (canDoubleJump)
			{
				canDoubleJump = false;
				velocity.Y = JumpVelocity * (float)delta;
			}
			
		} else if (Input.IsActionJustPressed(FastFallControl) && !IsOnFloor()) // Fast Fall
		{
			if (velocity.Y < 0) velocity = Vector2.Zero * (float)delta;
			velocity.Y += FastFallVelocity * (float)delta;
		}
		
		// Short hopping
		if (Input.IsActionJustReleased(JumpControl) && velocity.Y < -100)
		{
			velocity.Y = -100 * (float)delta;
		}

		return velocity;
	}

	public Vector2 Dash(Vector2 velocity, double delta)
	{
		if (Input.IsActionJustPressed(DashControl) && _canDash)
		{
			// do animation?
			GD.Print("dashing");
			GD.Print("dashing vel before:" + velocity);
			velocity = _dashDirection.Normalized() * DashForce * (float)delta;
			_canDash = false;
			_dashing = true;
			_lastDashMs = Time.GetTicksMsec();
			GD.Print("dashing vel after:" + velocity);
		}
		// GD.Print(velocity);
		// dash cooldown reset?
		if (!_canDash && (Time.GetTicksMsec() - _lastDashMs) > _dashCooldownMs)
		{
			// GD.Print("can dash again");
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

