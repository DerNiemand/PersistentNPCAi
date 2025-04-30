public abstract partial class OutOfViewBahviour : NPCBasic
{
    public override void _Ready()
    {
        base._Ready();
        EnableOfflineBehaviour();
    }
    public abstract void EnableOfflineBehaviour();
    
    public abstract void DisableOfflineBehaviour();
}
