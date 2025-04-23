using System;

public class TravelingState: NPCState
{


    public override void Process(double delta)
    {
        var nextPos = npc.GetNextNavPosition();
		var vectorToTarget = nextPos - npc.GlobalPosition;

		vectorToTarget = vectorToTarget.Normalized() * npc.MaxVelocity;

		if(npc.AvoidanceEnabled)
		{
			npc.SetNavAgentVelocity(vectorToTarget);
		}
		else
		{
			npc.Velocity = vectorToTarget;
			npc.MoveAndSlide();
		}
		npc.GetAllNPCsInView();
    }
}
