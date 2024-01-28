using Godot;
using System;

public partial class Devil : AnimatedSprite2D
{

	[Export] public float TextDelayMs = 3000;
	[Signal]
	public delegate void DevilStateChangeEventHandler(DevilState state);
	
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
			// Send back to game
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
