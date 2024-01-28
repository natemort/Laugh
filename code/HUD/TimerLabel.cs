using Godot;
using System;

public partial class TimerLabel : Label
{
	private Timer _timer;

	private Node2D _weaponPreview;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_timer = GetNode<Timer>("Timer");
		_weaponPreview = GetNode<Node2D>("WeaponPreview");
		if (GetTree().GetFirstNodeInGroup("PlayerManager") is PlayerManager playerManger)
		{
			playerManger.RoundStart += ResetTimer;
			playerManger.RoundEnd += StopTimer;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Double timeLeft = this.GetNode<Timer>("Timer").TimeLeft;
		int i = (int)timeLeft;
		this.Text = i.ToString();
	}

	public void TriggerWeaponSwap()
	{
		if (GetTree().GetFirstNodeInGroup("PlayerManager") is PlayerManager playerManger)
		{
			playerManger.SwapWeapons();
		}
		_UpdatePreview();
	}

	public void ResetTimer()
	{
		_UpdatePreview();
		_timer.Start();
	}

	public void StopTimer()
	{
		_ClearPreview();
		_timer.Stop();
	}

	private void _UpdatePreview()
	{
		if (GetTree().GetFirstNodeInGroup("PlayerManager") is PlayerManager playerManger)
		{
			_ClearPreview();
			_weaponPreview.AddChild(playerManger.PeekNextWeapon().Instantiate<Node2D>());
		}
	}

	private void _ClearPreview()
	{
		foreach (var child in _weaponPreview.GetChildren())
		{
			child.QueueFree();
		}
	}
}
