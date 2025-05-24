using Godot;
using System;
using System.Collections.Generic;

public partial class NPCEventSourcing : NPCBasic
{

	List<NPCBasic> alliesInView = new();

	double tradeCooldown = 0;
	double minTimeBetweenTrades = 5;

	public bool CanTrade
	{
		get => tradeCooldown <= 0;
	}

	new public bool EnemyIsInView
	{
		get
		{
			foreach (var ally in alliesInView)
			{
				viewRay.TargetPosition = ToLocal(ally.GlobalPosition);
				if (!viewRay.IsColliding())
				{
					return true;
				}
			}
			return false;
		}
	}
    EventStore eventStore;

    public override void _Ready()
    {
        base._Ready();
        eventStore = GetTree().GetFirstNodeInGroup("EventStore") as EventStore;
    }

    public override void _Process(double delta)
	{
		var nextState = currentState.Process(0.166);
		if (!string.IsNullOrEmpty(nextState))
		{
			ChangeState(nextState);
		}
		if (tradeCooldown > 0)
		{
			tradeCooldown -= 0.166;
		}
	}
	private void ChangeState(string state)
	{
		switch (state)
		{
			case "traveling":
				currentState.Exit();
				currentState = new TravelingState();
				currentState.Enter(this);
				break;
			case "combat":
				currentState.Exit();
				currentState = new TradingState();
				currentState.Enter(this);
				break;
			default:
				GD.Print("couldnt find spcified state to switch to");
				break;
		}
	}

	new public void OnBodyEnterViewArea(Node2D other)
	{
		if (other is NPCBasic)
		{
			var otherNPC = other as NPCBasic;
			npcsInView.Add(otherNPC);
			if (FactionStats.GetRelation(Faction, otherNPC.Faction) == Relation.Allies)
			{
				alliesInView.Add(otherNPC);
			}
		}
	}
	new public void OnBodyExitViewArea(Node2D other)
	{
		if (other is NPCBasic)
		{
			if (npcsInView.Contains(other as NPCBasic))
			{
				npcsInView.Remove(other as NPCBasic);
			}
			if (alliesInView.Contains(other as NPCBasic))
			{
				alliesInView.Remove(other as NPCBasic);
			}
		}
	}
}
