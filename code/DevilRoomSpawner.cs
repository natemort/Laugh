using Godot;
using System;
using Laugh.code;

public partial class DevilRoomSpawner : Node2D
{
	private static readonly String Pause = "\n                               \n";
	private static readonly NonRepeatingRandomSet<String> Jokes = new(
		new []
		{
			$"Why did the minus sign run for office?{Pause}To make a difference.",
			$"How many tickles does it take to make an octopus laugh?{Pause}Ten tickles.",
			$"Why did the cop pull over the U-Haul truck?{Pause}He was trying to bust a move.",
			$"I Burnt my Hawaiian pizza today.{Pause}Should have cooked it on aloha temperature",
			$"Scientists have been trying to talk to dolphins for years.{Pause}One day it just clicked.",
			$"A lot of people can turn into great drivers.{Pause}So if you're a great driver, look out for people turning.",
			$"I took ribeye to the top of Everest.{Pause}The steaks could not be higher.",
			$"Why do the riot police show up at concerts early?{Pause}To beat the crowd.",
			$"Why did the hungry baby calf cross the road?{Pause}To get to the udder side.",
			$"How do you know if your girlfriend is a keeper?{Pause}If she wears gloves, soccer jersey and cleats.",
			$"I have an inferiority complex{Pause}but it's not a very good one.",
			$"What type of investment do chemists prefer?{Pause}They have an affinity for bonds.",
			$"I bought a dozen cup-cakes from a bakery.{Pause}I got them home, opened the box and thought... that's odd.",
			$"Life is like a diploma{Pause}My parents keep telling me to get one.",
			$"What's the seamonster's favourite meal?{Pause}Fish & Ships"
		}
	);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var player = this.GetTransitionManager().GetWinner();
		this.AddChild(player);
		player.GlobalPosition = this.GetNode<Node2D>("PlayerSpawn").GlobalPosition;
		foreach (var joke in GetTree().GetNodesInGroup("Jokes"))
		{
			((Joke)joke).JokeText = Jokes.GetRandom();
		}
	}

}
