using Godot;
using System;
using System.Collections.Generic;
using Laugh.code;

public partial class ExplosionEvent : Node2D
{
	[Export] public int MinSpawnDelayMs = 300;
	[Export] public int MaxSpawnDelayMs = 3000;

	[Export] public int MinExplosionCount = 3;

	[Export] public int MaxExplosionCount = 10;

	[Export] public String SpawnGroup = "ExplosionSpawnPoints";

	[Export(PropertyHint.File, "*.tscn")]
	public String ToSpawn;


	private NonRepeatingRandomSet<Vector2> _spawnPoints;
	private PackedScene _toSpawn;
	private float _currentSpawnDelay;
	private float _lastSpawnMs;
	private int _targetSpawnCount;
	private int _spawnedCount = 0;

	public override void _Ready()
	{
		_lastSpawnMs = Time.GetTicksMsec();
		_currentSpawnDelay = Random.Shared.Next(MinSpawnDelayMs, MaxSpawnDelayMs);
		_targetSpawnCount = Random.Shared.Next(MinExplosionCount, MaxExplosionCount);
		_toSpawn = ResourceLoader.Load<PackedScene>(ToSpawn);
		_spawnPoints = GetSpawnPoints();
	}

	public override void _Process(double delta)
	{
		if ((Time.GetTicksMsec() - _lastSpawnMs - _currentSpawnDelay) > 0 && _spawnedCount < _targetSpawnCount)
		{
			Node2D explosion = _toSpawn.Instantiate<Node2D>();
			AddChild(explosion);
			explosion.GlobalPosition = _spawnPoints.GetRandom();
			_spawnedCount++;
			_lastSpawnMs = Time.GetTicksMsec();
			_currentSpawnDelay = Random.Shared.Next(MinSpawnDelayMs, MaxSpawnDelayMs);
		}
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

	public void OnChildExiting(Node node)
	{
		if (GetChildCount() == 1 && _spawnedCount == _targetSpawnCount)
		{
			this.QueueFree();
		}
	}
}
