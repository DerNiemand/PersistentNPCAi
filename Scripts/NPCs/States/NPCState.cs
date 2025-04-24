public class NPCState
{
    public NPCBasic npc;

    public virtual void Enter(NPCBasic npc)
    {
        this.npc = npc;
    }

    public virtual void Exit()
    {
        
    }
    public virtual string Process(double delta){ return "";}
    

    
}