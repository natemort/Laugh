using Godot;

namespace Laugh.code;

public static class Extensions
{
    public static TransitionManager GetTransitionManager(this Node node)
    {
        return node.GetNode<TransitionManager>("/root/TransitionManager");
    }
}