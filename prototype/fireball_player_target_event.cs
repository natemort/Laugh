using Godot;
using System;
using System.Collections.Generic;
using Laugh.code;

public partial class fireball_player_target_event : Node2D
{

	[Export] public int MaxOverallOfFireballs = 100;
	[Export] public int MinOverallOfFireballs = 50;

	[Export] public int MinFireballCount = 10;
	[Export] public int MaxFireballCount = 50;
	
	[Export] public int MinSpawnDelayMs = 0;
	[Export] public int MaxSpawnDelayMs = 1000;
	
	[Export] public PackedScene FireballScene;
	
	[Export] public String SpawnGroup = "FireballSpawnPoints";

	public float CurrentSpawnDelay;
	public float CurrentSpawDelayModifier = 1.0f;
	private NonRepeatingRandomSet<Vector2> _spawnPoints;
	private float _lastSpawnMs;
	private int _targetSpawnCount;
	private int _spawnedCount = 0;
	private float _currentSpawnDelay;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_spawnPoints = GetSpawnPoints();
		_lastSpawnMs = Time.GetTicksMsec();
		_currentSpawnDelay = Random.Shared.Next(MinSpawnDelayMs, MaxSpawnDelayMs);
		_targetSpawnCount = Random.Shared.Next(MinOverallOfFireballs, MaxOverallOfFireballs);
	}
	
	public NonRepeatingRandomSet<Vector2> GetSpawnPoints()
	{
		List<Vector2> points = new();
		foreach (Node node in GetTree().GetNodesInGroup(SpawnGroup))
		{
			if (node is Node2D node2D)
			{
				points.Add(node2D.GlobalPosition);
			}
		}

		return new NonRepeatingRandomSet<Vector2>(points);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Time.GetTicksMsec() - _lastSpawnMs > _currentSpawnDelay && _spawnedCount < _targetSpawnCount)
		{
			Node2D fireball = FireballScene.Instantiate<Node2D>();
			AddChild(fireball);
			fireball.GlobalPosition = _spawnPoints.GetRandom();
			_lastSpawnMs = Time.GetTicksMsec();
			_currentSpawnDelay = Random.Shared.Next(MinSpawnDelayMs, MaxSpawnDelayMs);
			_spawnedCount++;
		}
		
	}
}
