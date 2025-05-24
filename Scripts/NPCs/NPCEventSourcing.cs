using Godot;
using System;
using System.Collections.Generic;

public partial class NPCEventSourcing : NPCBasic
{
	Guid uid;
	public Guid Uid
	{
		get => uid;
	}
	List<NPCBasic> alliesInView = new();

	double tradeCooldown = 0;
	double minTimeBetweenTrades = 5;

	public bool CanTrade
	{
		get => tradeCooldown <= 0;
	}
	int money = 0;

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

	public EventStore EventStore
	{
		get => eventStore;
	}
    int currentEventIndex = 0;


	public override void _Ready()
	{
		base._Ready();
		eventStore = GetTree().GetFirstNodeInGroup("EventStore") as EventStore;
		uid = new();
		RandomNumberGenerator rng = new();
		money = rng.RandiRange(0, 10);
		
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

	public void AddMoney(int amount)
	{
		money += amount;
	}
	protected void ChangeState(string state)
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
			case "offline":
				currentState.Exit();
				currentState = new EventSourcingState();
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

#nullable enable
	public NPCEventSourcing? GetNearestAlly()
	{
		if (alliesInView.Count != 0)
		{
			NPCBasic closestEnemy = alliesInView[0];
			float closestEnemyDistanceSquared = float.MaxValue;
			foreach (var ally in alliesInView)
			{
				if (ally is not NPCEventSourcing)
				{
					continue;
				}
				var distanceSquaredToEnemy = GlobalPosition.DistanceSquaredTo(ally.GlobalPosition);
				if (distanceSquaredToEnemy < closestEnemyDistanceSquared)
				{
					closestEnemy = ally;
					closestEnemyDistanceSquared = distanceSquaredToEnemy;
				}
			}
			if (closestEnemy is NPCEventSourcing)
			{
				return closestEnemy as NPCEventSourcing;
			}
		}
		return null;
	}

#nullable disable

	public void StartTradeCooldown()
	{
		tradeCooldown = minTimeBetweenTrades;
	}
	
	protected void UpdateState()
	{
		var data = eventStore.GetNewerEventsInvolvingNPC(currentEventIndex, uid, out currentEventIndex);
		foreach (var entry in data)
		{
			switch (entry.eventType)
			{
				case EventType.TradeEvent:
					if (entry.data.TryGetValue("Money", out int tradeMoney))
					{
						if (entry.IsNPCInstigating(uid))
						{
							money += tradeMoney;
						}
						else
						{
							money -= tradeMoney;
						}
					}
					break;
				case EventType.MeetEvent:
				case EventType.DonationEvent:
				default:
					break;
			}
		}
	}
}
