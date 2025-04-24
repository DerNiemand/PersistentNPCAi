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
	public Vector2 targetPosition;
	[Export]
	Vector2 maxCoordinates = new(3856, 3936);
	NavigationAgent2D navAgent;


	NPCState currentState;

	[Export]
	private Weapon weapon;
	public float Range
	{
		get => weapon.Range;
	}
	public bool CanAttack{
		get => !weapon.AttackCoolingdown;
	}

	Area2D viewArea;
	RayCast2D viewRay;
	public bool AvoidanceEnabled
	{
		get { return navAgent.AvoidanceEnabled; }
	}
	[Export]
	private Faction faction = Faction.Independent;

	public Faction Faction
	{
		get => faction;
		set { if (faction == Faction.Independent) { faction = value; } }
	}

	List<NPCBasic> npcsInView = new();
	List<NPCBasic> enemiesInView = new();
	public bool EnemyIsInView
	{
		get
		{
			foreach (var enemy in enemiesInView)
			{
				viewRay.TargetPosition = ToLocal(enemy.GlobalPosition);
				if (!viewRay.IsColliding())
				{
					return true;
				}
			}
			return false;
		}
	}
	RandomNumberGenerator rng = new();
	public override void _Ready()
	{
		navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		viewArea = GetNode<Area2D>("ViewArea");
		viewRay = GetNode<RayCast2D>("ViewRay");
		currentState = new TravelingState();
		GetNode<AnimatedSprite2D>("Sprite2D").SpriteFrames = FactionStats.GetFactionSpriteFrames(faction);
		currentState.Enter(this);
		GetNewQuest();
	}

	public override void _Process(double delta)
	{
		var nextState = currentState.Process(delta);
		if (!string.IsNullOrEmpty(nextState))
		{
			ChangeState(nextState);
		}
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
			if (FactionStats.GetRelation(faction, otherNPC.faction) == Relation.Enemies)
			{
				enemiesInView.Add(otherNPC);
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
			if (enemiesInView.Contains(other as NPCBasic))
			{
				enemiesInView.Remove(other as NPCBasic);
			}
		}
	}

#nullable enable
	public NPCBasic? GetNearestEnemy()
	{
		if (enemiesInView.Count != 0)
		{
			NPCBasic closestEnemy = enemiesInView[0];
			float closestEnemyDistanceSquared = float.MaxValue;
			foreach (var enemy in enemiesInView)
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
#nullable disable

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
				currentState = new CombatState();
				currentState.Enter(this);
				break;
			default:
				GD.Print("couldnt find spcified state to switch to");
				break;
		}
	}

	public void Attack(Vector2 direction)
	{
		weapon.Attack(direction);
	}
}
