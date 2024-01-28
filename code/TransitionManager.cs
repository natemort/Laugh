using System;
using Godot;

namespace Laugh.code;

public partial class TransitionManager : Node2D
{

    public static readonly String FinalScene = "res://finish_room.tscn";
    public static readonly String ArenaScene = "res://code/HUD/ArenaScene.tscn";
    private String _winner;

    public void ReturnToArena()
    {
        _winner = null;
        GetTree().ChangeSceneToFile(ArenaScene);

    }

    public void SendToDevil(PlayerController2 player)
    {
        _winner = player.PlayerName;
        GetTree().ChangeSceneToFile(FinalScene);
    }

    public PlayerController2 GetWinner()
    {
        if (_winner == null)
        {
            throw new ArgumentException("Winner was never set!");
        }
        return ResourceLoader.Load<PackedScene>("res://prototype/" + _winner + ".tscn").Instantiate<PlayerController2>();
    }
    

}