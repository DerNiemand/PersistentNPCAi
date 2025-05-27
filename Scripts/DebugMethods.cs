using Godot;
using System;

public partial class DebugMethods : Node
{
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("debug_orphans"))
        {
            PrintOrphanNodes();
        }
        if (@event.IsActionPressed("debug_npccount"))
        {
            GD.Print(GetNode<Node2D>("../NPCs").GetChildCount());
        }
    }

}
