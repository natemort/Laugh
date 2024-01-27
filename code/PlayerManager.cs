using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerManager : Node2D
{
	private List<PlayerController2> _players;
	// Called when the node enters the scene tree for the first time.
	private Godot.Collections.Array<Node> spawnPoints;
	public override void _Ready()
	{
		GD.Print("Ready!!");
		_players = FindPlayers(GetTree().Root);

		spawnPoints = this.GetTree().GetNodesInGroup("SpawnPoints");
		
		foreach (var player in _players)
		{
			player.DeathSignal += OnPlayerDeath;
			int index = Random.Shared.Next() % spawnPoints.Count;
			Node2D n = (Node2D)spawnPoints[index];
			player.Position = n.Position;
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
		Respawn();
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
		}
	}
	
}
