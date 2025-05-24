using System;


public class EventSourcingState: NPCState
{
    NPCEventSourcing eventSourcingNPC;
    public override void Enter(NPCBasic npc)
    {
        eventSourcingNPC = npc as NPCEventSourcing;
    }
    public override string Process(double delta)
    {
        
        var nextPos = eventSourcingNPC.GetNextNavPosition();
		var vectorToTarget = nextPos - eventSourcingNPC.GlobalPosition;

		vectorToTarget = eventSourcingNPC.MaxVelocity * (float)delta * vectorToTarget.Normalized();
		

		if(eventSourcingNPC.AvoidanceEnabled)
		{
			eventSourcingNPC.SetNavAgentVelocity(vectorToTarget);
		}
		else
		{
			eventSourcingNPC.Velocity = vectorToTarget;
			eventSourcingNPC.MoveAndSlide();
		}

		if(eventSourcingNPC.EnemyIsInView && eventSourcingNPC.CanTrade)
		{

		}
        
        return "";
    }
}

