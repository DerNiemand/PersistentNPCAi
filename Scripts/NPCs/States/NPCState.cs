public class NPCState
{
    public NPCBasic npc;

    public void Init(NPCBasic npc)
    {
        this.npc = npc;
    }
    public virtual void Process(double delta){}
    

    
}