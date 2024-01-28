using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Laugh.code;

public partial class EventManager : Node2D
{
	[Export(PropertyHint.File, "*.tscn")] public String[] Events;

	[Export] public int EventDelayMinMs = 3_000;
	[Export] public int EventDelayMaxMs = 15_000;

	private bool _stopped = false;
	private NonRepeatingRandomSet<String> _events;
	private Node2D _currentEvent = null;

	private float _currentEventDelay;
	private float _lastEventEnd;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_events = new NonRepeatingRandomSet<string>(Events);
		ResumeEvents();
	}

	public override void _Process(double delta)
	{
		if (_stopped)
		{
			return;
		}
		if (_currentEvent == null && (Input.IsActionJustPressed("debug_event") || (Time.GetTicksMsec() - _lastEventEnd - _currentEventDelay) > 0))
		{
			_currentEvent = _CreateEvent();
			GD.Print("Started event: " + _currentEvent.Name);
			AddChild(_currentEvent);
		}
	}

	public void ResetEvents()
	{
		StopEvents();
		ResumeEvents();
	}

	public void ResumeEvents()
	{
		GD.Print("Resuming Events");
		_stopped = false;
		_lastEventEnd = Time.GetTicksMsec();
		_currentEventDelay = Random.Shared.Next(EventDelayMinMs, EventDelayMaxMs);
	}

	public void StopEvents()
	{
		GD.Print("Pausing Events");
		_stopped = true;
		foreach (Node child in GetChildren())
		{
			child.QueueFree();
		}
	}

	private Node2D _CreateEvent()
	{
		String toCreate = _events.GetRandom();
		return ResourceLoader.Load<PackedScene>(toCreate).Instantiate<Node2D>();
	}

	public void OnChildExiting(Node node)
	{
		GD.Print("Event ending: " + _currentEvent?.Name);
		if (node.Name == _currentEvent?.Name)
		{
			_currentEvent = null;
			_lastEventEnd = Time.GetTicksMsec();
		}
	}
	
}
