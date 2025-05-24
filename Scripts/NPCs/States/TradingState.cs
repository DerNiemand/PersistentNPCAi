
using System;

public class TradingState : NPCState
{
    public override string Process(double delta)
    {
        var target = npc.GetNearestEnemy();
        if (target != null)
        {
            if (npc.GlobalPosition.DistanceTo(target.GlobalPosition) < npc.Range)
            {
                if (npc.CanAttack)
                {
                    Trade();
                }
            }
            else
            {
                npc.SetNavAgentTarget(target.GlobalPosition);
                var nextPos = npc.GetNextNavPosition();
                var vectorToTarget = nextPos - npc.GlobalPosition;

                vectorToTarget = npc.MaxVelocity * (float)delta * vectorToTarget.Normalized();

                if (npc.AvoidanceEnabled)
                {
                    npc.SetNavAgentVelocity(vectorToTarget);
                }
                else
                {
                    npc.Velocity = vectorToTarget;
                    npc.MoveAndSlide();
                }
            }
            return "";
        }
        return "traveling";
    }

    private void Trade()
    {
        if (npc is NPCEventSourcing eventSourcingNPC)
        {
            eventSourcingNPC.AddMoney(2);
            eventSourcingNPC.StartTradeCooldown();
        }
    }
}