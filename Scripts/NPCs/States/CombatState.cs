using System;
using Godot;


public class CombatState : NPCState
{
    NPCBasic target;
    public override string Process(double delta)
    {
        target = npc.GetNearestEnemy();
        if (target != null)
        {
            if (npc.GlobalPosition.DistanceTo(target.GlobalPosition) < npc.Range)
            {
                if (npc.CanAttack)
                {
                    Attack();
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

    private void Attack()
    {
        npc.Attack(target.GlobalPosition - npc.GlobalPosition);
    }
}