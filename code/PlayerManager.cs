using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerManager : Node2D
{
	private List<PlayerController2> _players;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Ready!!");
		_players = FindPlayers(GetTree().Root);
		foreach (var player in _players)
		{
			player.DeathSignal += OnPlayerDeath;
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
	
}
