using System;
using Godot;


public class EventSourcingState: NPCState
{
    NPCEventSourcing eventSourcingNPC;

	RandomNumberGenerator rng;
	public override void Enter(NPCBasic npc)
	{
		eventSourcingNPC = npc as NPCEventSourcing;
		rng = new();
	}
	
    public override string Process(double delta)
	{

		var nextPos = eventSourcingNPC.GetNextNavPosition();
		var vectorToTarget = nextPos - eventSourcingNPC.GlobalPosition;

		vectorToTarget = eventSourcingNPC.MaxVelocity * (float)delta * vectorToTarget.Normalized();


		if (eventSourcingNPC.AvoidanceEnabled)
		{
			eventSourcingNPC.SetNavAgentVelocity(vectorToTarget);
		}
		else
		{
			eventSourcingNPC.Velocity = vectorToTarget;
			eventSourcingNPC.MoveAndSlide();
		}

		if (eventSourcingNPC.EnemyIsInView && eventSourcingNPC.CanTrade)
		{
			var tradingNpc = eventSourcingNPC.GetNearestAlly();
			if (tradingNpc != null)
			{
				Event newEvent = new(EventType.TradeEvent, eventSourcingNPC.Uid,
									 new() { tradingNpc.Uid }, new() { ["Money"] = rng.RandiRange(0, 5) });
				eventSourcingNPC.EventStore.AddEvent(newEvent);
				eventSourcingNPC.StartTradeCooldown();
			}
		}

		return "";
	}
}

