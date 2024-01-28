using Godot;
using System;

public partial class Joke : Area2D
{
	[Signal]
	public delegate void JokeSelectedEventHandler();

	public String JokeText;
	private AnimatedText _text;
	private bool _ready = false;
	private bool _selected = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_text = GetNode<AnimatedText>("JokeText");
	}

	public void OnDevilStateChange(int rawState)
	{
		GD.Print("Got called?");
		Devil.DevilState state = (Devil.DevilState)rawState;
 		if (state == Devil.DevilState.AwaitingResponse)
		{
			SelectJoke();
		} else if (state == Devil.DevilState.Responding)
	    {
		  _text.Clear();  
	    } else if (state == Devil.DevilState.Finished)
		{
			if (_selected)
			{
				_text.Speak("Uh oh...");
			}
		}
	}

	public void SelectJoke()
	{
		_text.Speak(JokeText);
	}

	public void OnTextFinished()
	{
		GD.Print("On text finished: " + Name);
		_ready = true;
	}

	public void OnBodyEntered(Node2D node)
	{
		GD.Print("On body entered?");
		if (_ready && !_selected)
		{
			GD.Print("selected!");
			_selected = true;
			_text.Clear();
			EmitSignal(SignalName.JokeSelected);
		}
	}
}
