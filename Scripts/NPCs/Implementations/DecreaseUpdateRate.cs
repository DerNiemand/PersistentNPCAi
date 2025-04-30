using System;
using Godot;

public partial class DecreaseUpdateRate : OutOfViewBahviour
{
    [Export]
    Timer offlineTickTimer;
    [Export]
    double offlineTickTime = 2.0;

    private float pathDesiredDistanceOriginal;
    public override void DisableOfflineBehaviour()
    {
        SetProcess(true);
        offlineTickTimer.Stop();
        GetNode<NavigationAgent2D>("NavigationAgent2D").PathDesiredDistance = pathDesiredDistanceOriginal;
        SetCollisionMaskValue(1,true);
    }

    public override void EnableOfflineBehaviour()
    {
        SetProcess(false);
        offlineTickTimer.Start(offlineTickTime);
        pathDesiredDistanceOriginal = GetNode<NavigationAgent2D>("NavigationAgent2D").PathDesiredDistance;
        GetNode<NavigationAgent2D>("NavigationAgent2D").PathDesiredDistance = MaxVelocity * 0.1f;
        SetCollisionMaskValue(1,false);
    }

    public void OfflineProccess()
    {
        _Process(offlineTickTime);
    }
}
