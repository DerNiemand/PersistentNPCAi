using Godot;
using System;

public partial class DebugMethods : Node
{
    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("debug_orphans"))
		{
			PrintOrphanNodes();
		}
    }

}
