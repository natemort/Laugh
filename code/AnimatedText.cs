using Godot;
using System;

public partial class AnimatedText : RichTextLabel
{
	[Export] public float CharactersPerMs = 0.1f;
	
	[Signal]
	public delegate void TextFinishedEventHandler();

	private float _startTime;
	private bool _done;
	
	public override void _Ready()
	{
		this.VisibleRatio = 0;
	}

	public override void _Process(double delta)
	{
		if (_startTime > 0)
		{
			float msElapsed = Time.GetTicksMsec() - _startTime;
			float percentVisible = msElapsed * CharactersPerMs / Text.Length;
			VisibleRatio = Mathf.Clamp(percentVisible, 0, 1);
			if (VisibleRatio >= 1 && !_done)
			{
				_done = true;
				EmitSignal(SignalName.TextFinished);
			}
		}
	}

	public void Speak(String text)
	{
		Text = text;
		_startTime = Time.GetTicksMsec();
		VisibleRatio = 0;
		_done = false;
	}

	public void Clear()
	{
		Text = "";
		_startTime = 0;
		VisibleRatio = 0;
		_done = false;
	}
}
