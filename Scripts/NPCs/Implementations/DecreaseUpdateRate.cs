using System;
using Godot;

public partial class DecreaseUpdateRate : OutOfViewBahviour
{
    [Export]
    Timer offlineTickTimer;
    [Export]
    double offlineTickTime = 2.0;

    private float pathDesiredDistanceOriginal = 5.0f;
    public override void DisableOfflineBehaviour()
    {
        SetProcess(true);
        offlineTickTimer.Stop();
        GetNode<NavigationAgent2D>("NavigationAgent2D").PathDesiredDistance = pathDesiredDistanceOriginal;
        SetCollisionMaskValue(1, true);
        GetNode<AnimatedSprite2D>("Sprite2D").Visible = true;
        GetNode<Weapon>("Sword").EnableVisuals();
    }

    public override void EnableOfflineBehaviour()
    {
        SetProcess(false);
        offlineTickTimer.Start(offlineTickTime);
        pathDesiredDistanceOriginal = GetNode<NavigationAgent2D>("NavigationAgent2D").PathDesiredDistance;
        GetNode<NavigationAgent2D>("NavigationAgent2D").PathDesiredDistance = MaxVelocity * 0.1f;
        SetCollisionMaskValue(1, false);
        GetNode<AnimatedSprite2D>("Sprite2D").Visible = false;
        GetNode<Weapon>("Sword").DisableVisuals();
    }

    public void OfflineProccess()
    {
        _Process(offlineTickTime);
    }
}
