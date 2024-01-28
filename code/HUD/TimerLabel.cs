using Godot;
using System;

public partial class TimerLabel : Label
{
	private Timer _timer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_timer = GetNode<Timer>("Timer");
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
		GetTree().GetFirstNodeInGroup("PlayerManager")?.Call("SwapWeapons");
	}

	public void ResetTimer()
	{
		_timer.Start();
	}

	public void StopTimer()
	{
		_timer.Stop();
	}
}
