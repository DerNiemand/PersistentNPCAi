using System.Collections.Generic;
using Godot;

public partial class NPCBasic : CharacterBody2D, PersistentNPC
{
	[Export]
	float maxVelocity;
	public float MaxVelocity
	{
		get { return maxVelocity; }
	}

	[Export]
	Vector2 targetPosition;
	[Export]
	Vector2 maxCoordinates = new Vector2(3856, 3936);
	NavigationAgent2D navAgent;

	Area2D viewArea;
	RayCast2D viewRay;
	public bool AvoidanceEnabled
	{
		get { return navAgent.AvoidanceEnabled; }
	}
	RandomNumberGenerator rng = new RandomNumberGenerator();
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
		foreach (var node in viewArea.GetOverlappingBodies())
		{
			if (node is not NPCBasic)
			{
				break;
			}
			viewRay.TargetPosition = ToLocal(node.GlobalPosition);
			if (!viewRay.IsColliding())
			{
				retVal.Add(node as NPCBasic);
			}
		}

		return retVal;
	}
}
