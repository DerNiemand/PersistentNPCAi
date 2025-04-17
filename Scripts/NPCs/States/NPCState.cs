public class NPCState
{
    NPCBasic npc;

    public void Init(NPCBasic npc)
    {
        this.npc = npc;
    }
    public virtual void Process(double delta){}
    

    
}