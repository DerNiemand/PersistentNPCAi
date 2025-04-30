
public class TravelingState: NPCState
{
	public override void Enter(NPCBasic npc)
	{
		base.Enter(npc);
		npc.SetNavAgentTarget(npc.targetPosition);
	}

    public override string Process(double delta)
    {
        var nextPos = npc.GetNextNavPosition();
		var vectorToTarget = nextPos - npc.GlobalPosition;

		vectorToTarget = npc.MaxVelocity * (float)delta * vectorToTarget.Normalized();
		

		if(npc.AvoidanceEnabled)
		{
			npc.SetNavAgentVelocity(vectorToTarget);
		}
		else
		{
			npc.Velocity = vectorToTarget;
			npc.MoveAndSlide();
		}
		if(npc.EnemyIsInView)
		{
			return "combat";
		}
		return "";
    }
}
