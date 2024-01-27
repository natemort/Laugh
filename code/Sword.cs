using Godot;
using System;
using Laugh.code;

public partial class Sword : Node2D, Weapon
{
	[Export]
	public float HitTimeMs = 500;
	[Export]
	public float RecoverTimeMs = 1000;
	[Export] public float SlashRotationDegrees = 70;
	private Sprite2D _slashEffect;
	private Sprite2D _swordSprite;
	private Area2D _hitbox;
	private Node2D _rotationPoint;
	private Vector2 _startPoint;
	private SwordState _state;
	private double _stateChangeTime;
	public override void _Ready()
	{
		_slashEffect = GetNode<Sprite2D>("SlashEffect");
		_swordSprite = GetNode<Sprite2D>("SwordSprite");
		_hitbox = GetNode<Area2D>("Hitbox");
		_rotationPoint = GetNode<Node2D>("RotationPoint");
		
		_hitbox.SetProcess(false);
		_slashEffect.Visible = false;
		_startPoint = _swordSprite.Position;
	}

	public override void _Process(double delta)
	{
		if (_state == SwordState.Attacking && (Time.GetTicksMsec() - _stateChangeTime) >= HitTimeMs)
		{
			_state = SwordState.Recovering;
			_stateChangeTime = Time.GetTicksMsec();
			_hitbox.SetProcess(false);
			_slashEffect.Visible = false;
		} else if (_state == SwordState.Recovering && (Time.GetTicksMsec() - _stateChangeTime) >= RecoverTimeMs)
		{
			_state = SwordState.Ready;
			_stateChangeTime = Time.GetTicksMsec();
			_swordSprite.RotationDegrees = 0;
			GD.Print("start: " + _startPoint);
			_swordSprite.Position = _startPoint;
		}
	}

	public void Fire()
	{
		if (_state == SwordState.Ready)
		{
			_state = SwordState.Attacking;
			_stateChangeTime = Time.GetTicksMsec();
			_hitbox.SetProcess(true);
			_slashEffect.Visible = true;
			GD.Print("Start position: " + _swordSprite.GlobalPosition);
			_swordSprite.Position = _rotationPoint.Position;
			_swordSprite.RotationDegrees = SlashRotationDegrees;
			// 60, 280
			GD.Print("End Position: " + _swordSprite.GlobalPosition);
		}
		
	}

	enum SwordState
	{
		Ready,
		Attacking,
		Recovering,
	}
}
