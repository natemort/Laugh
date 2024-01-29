using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using Laugh.code;
using Array = Godot.Collections.Array;

public partial class FireballRowEvent : Node2D
{
	[Export] public String BotSpawnGroup = "FireballBottomRowSpawns";
	[Export] public String RightSpawnGroup = "FireballRightSpawns";
	[Export] public String LeftSpawnGroup = "FireballLeftSpawns";
	[Export] public String TopSpawnGroup = "FireballTopSpawns";
	
	// private Array<Node> _topSpawnPoints;
	// private Array<Node> _botSpawnPoints;
	// private Array<Node> _rightSpawnPoints;
	// private Array<Node> _leftSpawnPoints;
	private Array<Array<Node>> _spawnSet = new();
	// private Dictionary<int, Vector2> = new Godot.Collections.Dictionary<int, Vector2>();
	private Godot.Collections.Dictionary<int, Vector2> indexToDirection = new();
	
	[Export] public int MaxOverallWaves = 8;
	[Export] public int MinOverallWaves = 3;
	
	[Export] public int MinSpawnDelayMs = 3000;
	[Export] public int MaxSpawnDelayMs = 10000;
	[Export] public int NodeSpawnPercentage = 80;
	
	[Export] public PackedScene FireballStraightScene;
	
	private float _lastSpawnMs;
	private int _targetWaveCount;
	private int _waveSpawnedCount = 0;
	private float _currentSpawnDelay;
	
	private ArenaCamera _camera;
	private Array<Node> _cameraTargets;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_populateSpawnPointSets();
		_lastSpawnMs = Time.GetTicksMsec();
		_currentSpawnDelay = 1000f;
		_targetWaveCount = Random.Shared.Next(MinOverallWaves, MaxOverallWaves);
		
		_camera = (ArenaCamera)GetTree().GetFirstNodeInGroup("CameraGroup");
		_cameraTargets = GetTree().GetNodesInGroup("FireballCamera");
		foreach (Node2D node in _cameraTargets)
		{
			_camera.addTarget(node);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Time.GetTicksMsec() - _lastSpawnMs > _currentSpawnDelay && _waveSpawnedCount < _targetWaveCount)
		{
			// spawn 80% of nodes
			int spawnIndex = Random.Shared.Next(0, _spawnSet.Count);
			foreach(Node2D spawn in _spawnSet[spawnIndex])
			{
				GD.Print(NodeSpawnPercentage + " vs " + Random.Shared.Next(0, 100));
				if (NodeSpawnPercentage > Random.Shared.Next(0, 100))
				{
					FireballStraight fireball = (FireballStraight)FireballStraightScene.Instantiate<Node2D>();
					fireball.GlobalPosition = spawn.GlobalPosition;
					fireball.Direction = indexToDirection[spawnIndex];
					
					AddChild(fireball);
				}
			}
			// fireball.GlobalPosition = _spawnPoints.GetRandom();
			// AddChild(fireball);
			// fireball.GlobalPosition = _spawnPoints.GetRandom();
			_lastSpawnMs = Time.GetTicksMsec();
			_currentSpawnDelay = Random.Shared.Next(MinSpawnDelayMs, MaxSpawnDelayMs);
			_waveSpawnedCount++;
		}
	}

	private void _populateSpawnPointSets()
	{
		_spawnSet.Add(_getSpawnPointsFromGroup(BotSpawnGroup));
		indexToDirection.Add(0, Vector2.Up);
		_spawnSet.Add(_getSpawnPointsFromGroup(TopSpawnGroup));
		indexToDirection.Add(1, Vector2.Down);
		_spawnSet.Add(_getSpawnPointsFromGroup(RightSpawnGroup));
		indexToDirection.Add(2, Vector2.Left);
		_spawnSet.Add(_getSpawnPointsFromGroup(LeftSpawnGroup));
		indexToDirection.Add(3, Vector2.Right);
	}

	private Array<Node> _getSpawnPointsFromGroup(String groupName)
	{
		return this.GetTree().GetNodesInGroup(groupName);
	}
	
	public void _on_child_exiting_tree(Node2D node)
	{
		if (GetChildCount() == 1 && _waveSpawnedCount == _targetWaveCount)
		{
			foreach (Node2D n in _cameraTargets)
			{
				_camera.removeTarget(n);
			}
			this.QueueFree();
		}
	}

	public void _on_tree_exiting()
	{
		foreach (Node2D n in _cameraTargets)
		{
			_camera.removeTarget(n);
		}
	}
}
