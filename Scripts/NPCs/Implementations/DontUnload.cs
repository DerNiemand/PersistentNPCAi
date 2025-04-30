using System;
using Godot;

public partial class DontUnload : OutOfViewBahviour
{
    public override void DisableOfflineBehaviour()
    {
        GetNode<AnimatedSprite2D>("Sprite2D").Visible = true;
        GetNode<Weapon>("Sword").EnableVisuals();
    }

    public override void EnableOfflineBehaviour()
    {
        GetNode<AnimatedSprite2D>("Sprite2D").Visible = false;
        GetNode<Weapon>("Sword").DisableVisuals();
    }
}
