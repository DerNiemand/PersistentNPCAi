using Godot;

public partial class TurnBased : OutOfViewBahviour
{
	[Signal]
	public delegate void OfllineBehaviourEnabledEventHandler(TurnBased npc);
	[Signal]
	public delegate void OfllineBehaviourDisabledEventHandler(TurnBased npc);

    private float pathDesiredDistanceOriginal = 5.0f;

	public override void DisableOfflineBehaviour()
	{
		SetProcess(true);
		SetCollisionMaskValue(1, true);
        GetNode<NavigationAgent2D>("NavigationAgent2D").PathDesiredDistance = pathDesiredDistanceOriginal;
		GetNode<AnimatedSprite2D>("Sprite2D").Visible = true;
		GetNode<Weapon>("Sword").EnableVisuals();
		EmitSignal(SignalName.OfllineBehaviourDisabled, this);
	}

	public override void EnableOfflineBehaviour()
	{
		SetProcess(false);
		SetCollisionMaskValue(1, false);
        pathDesiredDistanceOriginal = GetNode<NavigationAgent2D>("NavigationAgent2D").PathDesiredDistance;
        GetNode<NavigationAgent2D>("NavigationAgent2D").PathDesiredDistance = MaxVelocity * 0.1f;
		GetNode<AnimatedSprite2D>("Sprite2D").Visible = false;
		GetNode<Weapon>("Sword").DisableVisuals();
		EmitSignal(SignalName.OfllineBehaviourEnabled, this);
	}

}
