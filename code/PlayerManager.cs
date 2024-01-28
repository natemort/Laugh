using Godot;
using System;
using System.Collections.Generic;
using Laugh.code;

public partial class PlayerManager : Node2D
{
	private List<PlayerController2> _players;
	// Called when the node enters the scene tree for the first time.
	private Godot.Collections.Array<Node> spawnPoints;
	[Signal]
	public delegate void RoundStartEventHandler();

	[Signal]
	public delegate void RoundRespawnEventHandler();

	[Signal]
	public delegate void RoundEndEventHandler();

	[Export] public PackedScene[] Weapons;

	private NonRepeatingRandomSet<PackedScene> _weapons;
	private float _currentPauseTimeMs;
	private float _waitTimeMs = 1500;
	private RespawnState _respawnState = RespawnState.none;
	
	private enum RespawnState
	{
		none,
		death,
		preSpawn
	}
	public override void _Ready()
	{
		GD.Print("Ready!!");
		_players = FindPlayers(GetTree().Root);

		spawnPoints = this.GetTree().GetNodesInGroup("SpawnPoints");
			GD.Print(spawnPoints);
		
		foreach (var player in _players)
		{
			player.DeathSignal += OnPlayerDeath;

			int index = Random.Shared.Next() % spawnPoints.Count;
			Node2D n = (Node2D)spawnPoints[index];
			player.Position = n.Position;
			
		}

		_weapons = new NonRepeatingRandomSet<PackedScene>(Weapons);
		SwapWeapons();
	}

	public override void _Process(double delta)
	{
		
		// if there was a player death, set their phsyics to nothing and enable death pause
		if (_respawnState == RespawnState.death && Time.GetTicksMsec() - _currentPauseTimeMs >= _waitTimeMs)
		{
			// now respawn them
			EmitSignal(SignalName.RoundRespawn);
			Respawn();
			SwapWeapons();
		} else if (_respawnState == RespawnState.preSpawn && Time.GetTicksMsec() - _currentPauseTimeMs >= _waitTimeMs)
		{
			EmitSignal(SignalName.RoundStart);
			ResumePlayerProcessing();
		}
	}


	public void OnPlayerDeath(PlayerController2 player)
	{
		foreach (PlayerController2 other in _players)
		{
			if (other.PlayerName != player.PlayerName)
			{
				other.Health++;
			}
		}
		StopPlayerProcessing();
	}

	private List<PlayerController2> FindPlayers(Node node)
	{
		List<PlayerController2> result = new();
		foreach (Node child in node.GetChildren())
		{
			if (child is PlayerController2 player)
			{
				GD.Print("Found: " + player.PlayerName);
				result.Add(player);
			}
			else
			{
				result.AddRange(FindPlayers(child));
			}
		}

		return result;
	}

	private void StopPlayerProcessing()
	{
		foreach (PlayerController2 player in _players)
		{
			GD.Print("disable player: " + player.PlayerName);
			player.Freeze = true;
		}
		_currentPauseTimeMs = Time.GetTicksMsec();
		_respawnState = RespawnState.death;
		EmitSignal(SignalName.RoundEnd);
	}

	private void ResumePlayerProcessing()
	{
		foreach (PlayerController2 player in _players)
		{
			GD.Print("reenable player: " + player.PlayerName);
			player.Freeze = false;
		}

		_respawnState = RespawnState.none;
	}

	private void Respawn()
	{

		HashSet<int> s = new HashSet<int>();
		foreach (PlayerController2 player in _players)
		{
			int index = Random.Shared.Next() % spawnPoints.Count;
			while (s.Contains(index))
			{
				index = Random.Shared.Next() % spawnPoints.Count;
			}

			s.Add(index);
			Node2D n = (Node2D)spawnPoints[index];
			player.Position = n.Position;
			player.Alive = true;
		}
		
		// delay again
		_currentPauseTimeMs = Time.GetTicksMsec();
		_respawnState = RespawnState.preSpawn;
	}

	public void SwapWeapons()
	{
		PackedScene weapon = _weapons.GetRandom();
		foreach (PlayerController2 player in _players)
		{
			player.SetWeapon(weapon.Instantiate<Node2D>());
		}
	}
}
