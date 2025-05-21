
public interface IPersistentNPC
{
    public abstract void GetNewQuest();
    public virtual void AbandonQuest()
    {
        GetNewQuest();
    }
}