using Godot;
using System;
using Laugh.code;

public partial class Devil : AnimatedSprite2D
{

	[Export] public float TextDelayMs = 2000;
	[Export] public String ArenaName = "res://code/HUD/ArenaScene.tscn";
	[Signal]
	public delegate void DevilStateChangeEventHandler(DevilState state);

	private PackedScene _arena;
	private DevilState _state = DevilState.Prompting;
	private AnimatedText _text;

	public DevilState State
	{
		get
		{
			return _state;
		}
		set
		{
			_lastStateChange = Time.GetTicksMsec();
			_state = value;
			GD.Print("Emitting Signal");
			EmitSignal(SignalName.DevilStateChange, (int) _state);
		}
	}

	private float _lastStateChange;
	public override void _Ready()
	{
		_text = GetNode<AnimatedText>("DevilText");
		_text.Speak("Tell me your best joke");
		_arena = ResourceLoader.Load<PackedScene>(ArenaName);
	}

	public override void _Process(double delta)
	{
		if (State == DevilState.RespondingDelay && (Time.GetTicksMsec() - _lastStateChange) > TextDelayMs)
		{
			_text.Speak("I'm disappointed");
			Play("idle");
			State = DevilState.Responding2;
		} else if (State == DevilState.Finished && (Time.GetTicksMsec() - _lastStateChange) > TextDelayMs)
		{
			this.GetTransitionManager().ReturnToArena();
		}
	}

	public void OnTextFinished()
	{
		if (State == DevilState.Prompting)
		{
			State = DevilState.AwaitingResponse;
		} else if (State == DevilState.Responding)
		{
			State = DevilState.RespondingDelay;
			
		} else if (State == DevilState.Responding2)
		{
			State = DevilState.Finished;
		}
	}

	public void OnJokeSelected()
	{
		_text.Speak("That's it?");
		Play("dead");
		State = DevilState.Responding;
	}

	public enum DevilState
	{
		Prompting,
		AwaitingResponse,
		Responding,
		RespondingDelay,
		Responding2,
		Finished,
	}
}
