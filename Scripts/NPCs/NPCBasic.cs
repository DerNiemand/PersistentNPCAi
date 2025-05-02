using System.Collections.Generic;
using Godot;

public partial class NPCBasic : CharacterBody2D, PersistentNPC
{
	[Export, ExportCategory("Stats")]
	int maxHealth = 5;
	int health;
	public int Health
	{
		get => health;
	}

	[Export]
	float maxVelocity;
	public float MaxVelocity
	{
		get => maxVelocity;
	}

	public Vector2 targetPosition;
	NavigationAgent2D navAgent;
	public Vector2 navTargetPosition;
	public Vector2 NavTargetPosition
	{
		get
		{
			if (navTargetPositionDirty)
			{
				navTargetPosition = navAgent.GetFinalPosition();
				navTargetPositionDirty = false;
			}
			return navTargetPosition;
		}
	}
	private bool navTargetPositionDirty = true;

	NPCState currentState;

	[Export]
	private Weapon weapon;
	public float Range
	{
		get => weapon.Range;
	}
	public bool CanAttack
	{
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
		GetNewQuest();
		currentState.Enter(this);
		health = maxHealth;
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
		SetNavAgentTarget(targetPosition);
	}

	public void GetNewQuest()
	{
		var questManager = (QuestManager)GetTree().GetFirstNodeInGroup("QuestManager");
		var quest = questManager.GetQuest(faction, GlobalPosition);
		targetPosition = quest.Location;
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
	public void OnNavigationAgentPathChanged()
	{
		navTargetPositionDirty = true;
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

	public void GetHitByNPC(int damage, Faction hittingFaction)
	{
		if (FactionStats.GetRelation(faction, hittingFaction) == Relation.Enemies)
		{
			health -= damage;
			if (health <= 0)
			{
				Position = new(-1000, -1000);
				QueueFree();
			}
		}
	}

	public void OnNPCHit(NPCBasic hitNPC, int damage)
	{
		hitNPC.GetHitByNPC(damage, faction);
	}
}
