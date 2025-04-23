using System.Collections.Generic;
using Godot;

public partial class NPCBasic : CharacterBody2D, PersistentNPC
{
	[Export]
	float maxVelocity;
	public float MaxVelocity
	{
		get => maxVelocity;
	}

	[Export]
	Vector2 targetPosition;
	[Export]
	Vector2 maxCoordinates = new(3856, 3936);
	NavigationAgent2D navAgent;

	[Export]
	float range;
	public float Range
	{
		get => range;
	}

	Area2D viewArea;
	RayCast2D viewRay;
	public bool AvoidanceEnabled
	{
		get { return navAgent.AvoidanceEnabled; }
	}
	private Faction faction = Faction.Independent;

	public Faction Faction
	{
		get => faction;
		set { if (faction == Faction.Independent) { faction = value; } }
	}

	List<NPCBasic> npcsInView = new();
	List<NPCBasic> enemeisInView = new();
	RandomNumberGenerator rng = new();
	public override void _Ready()
	{
		navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		viewArea = GetNode<Area2D>("ViewArea");
		viewRay = GetNode<RayCast2D>("ViewRay");
		GetNewQuest();
	}

	public override void _Process(double delta)
	{
		var nextPos = navAgent.GetNextPathPosition();
		var vectorToTarget = nextPos - GlobalPosition;

		vectorToTarget = vectorToTarget.Normalized() * MaxVelocity;

		if (navAgent.AvoidanceEnabled)
		{
			SetNavAgentVelocity(vectorToTarget);
		}
		else
		{
			Velocity = vectorToTarget;
			MoveAndSlide();
		}
		GetAllNPCsInView();
	}

	public void OnSafeVelocityCalculated(Vector2 safeVelocity)
	{
		Velocity = safeVelocity;
		MoveAndSlide();
	}

	public void NavigationFinished()
	{
		GetNewQuest();
	}

	public void GetNewQuest()
	{
		var questManager = (QuestManager)GetTree().GetFirstNodeInGroup("QuestManager");
		var quest = questManager.GetQuest(GlobalPosition);
		navAgent.TargetPosition = quest.Location;
	}

	public void SetNavAgentTarget(Vector2 targetPosition)
	{
		navAgent.TargetPosition = targetPosition;
	}
	public Vector2 GetNextNavPosition()
	{
		return navAgent.GetNextPathPosition();
	}
	public void SetNavAgentVelocity(Vector2 velocity)
	{
		navAgent.SetVelocity(velocity);
	}

	public List<NPCBasic> GetAllNPCsInView()
	{
		List<NPCBasic> retVal = new();
		foreach (var npc in npcsInView)
		{
			viewRay.TargetPosition = ToLocal(npc.GlobalPosition);
			if (!viewRay.IsColliding())
			{
				retVal.Add(npc);
			}
		}

		return npcsInView;
	}

	public void OnBodyEnterViewArea(Node2D other)
	{
		if (other is NPCBasic)
		{
			var otherNPC = other as NPCBasic;
			npcsInView.Add(otherNPC);
			if (FactionRelations.GetRelation(faction, otherNPC.faction) == Relation.Enemies)
			{
				enemeisInView.Add(otherNPC);
			}
		}
	}

	public void OnBodyExitViewArea(Node2D other)
	{
		if (other is NPCBasic)
		{
			if (npcsInView.Contains(other as NPCBasic))
			{
				npcsInView.Remove(other as NPCBasic);
			}
			if (enemeisInView.Contains(other as NPCBasic))
			{
				enemeisInView.Remove(other as NPCBasic);
			}
		}
	}

	public NPCBasic? GetNearestEnemy()
	{
		if (enemeisInView.Count != 0)
		{
			NPCBasic closestEnemy = new();
			float closestEnemyDistanceSquared = float.MaxValue;
			foreach (var enemy in enemeisInView)
			{
				var distanceSquaredToEnemy = GlobalPosition.DistanceSquaredTo(enemy.GlobalPosition);
				if (distanceSquaredToEnemy < closestEnemyDistanceSquared)
				{
					closestEnemy = enemy;
					closestEnemyDistanceSquared = distanceSquaredToEnemy;
				}
			}
			return closestEnemy;
		}
		return null;
	}

}
