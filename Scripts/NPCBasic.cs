using Godot;

public partial class NPCBasic : CharacterBody2D
{
	[Export]
	float maxVelocity;

	[Export]
	Vector2 targetPosition;
	[Export]
	Vector2 maxCoordinates = new Vector2(3856,3936);
	NavigationAgent2D navAgent;
	RandomNumberGenerator rng = new RandomNumberGenerator();
	public override void _Ready()
	{
		navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		navAgent.TargetPosition = targetPosition;
		
	}

	public override void _Process(double delta)
	{
		var nextPos = navAgent.GetNextPathPosition();
		var vectorToTarget = nextPos - GlobalPosition;

		vectorToTarget = vectorToTarget.Normalized() * maxVelocity;

		if(navAgent.AvoidanceEnabled)
		{
			navAgent.SetVelocity(vectorToTarget);
		}
		else
		{
			Velocity = vectorToTarget;
			MoveAndSlide();
		}
	    
	}

	public void OnSafeVelocityCalculated(Vector2 safeVelocity)
	{
		Velocity = safeVelocity;
		MoveAndSlide();
	}

	public void TargetReached()
	{
		GD.Print("Navigation finished");
	}

	public void NavigationFinished()
	{
		targetPosition.X = rng.RandfRange(0, maxCoordinates.X);
		targetPosition.Y = rng.RandfRange(0, maxCoordinates.Y);
		navAgent.TargetPosition = targetPosition;
	}
}
