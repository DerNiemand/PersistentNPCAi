using Godot;

public partial class EventSourcing : NPCEventSourcing
{
    public override void _Ready()
    {
        base._Ready();
        EnableOfflineBehaviour();
    }

    public void DisableOfflineBehaviour()
    {
        ChangeState("traveling");
        GetNode<AnimatedSprite2D>("Sprite2D").Visible = true;
        GetNode<Weapon>("Sword").EnableVisuals();
        UpdateState();
    }

    public void EnableOfflineBehaviour()
    {
        ChangeState("offline");
        GetNode<AnimatedSprite2D>("Sprite2D").Visible = false;
        GetNode<Weapon>("Sword").DisableVisuals();
    }
}