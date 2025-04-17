
public interface PersistentNPC
{
    public abstract void GetNewQuest();
    public virtual void AbandonQuest()
    {
        GetNewQuest();
    }
}