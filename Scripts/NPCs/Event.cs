using System.Collections.Generic;
using System.Linq;

public class Event
{
    private EventType eventType;
    private int instigatingNPC;
    private List<int> involvedNPCs;
    private List<int> updatedNPCs = new();
    private bool AllNpcsUpdated
    {
        get
        {


            if (updatedNPCs.Contains(instigatingNPC))
            {
                if (updatedNPCs.Intersect(involvedNPCs).Count() == involvedNPCs.Count - 1)
                {
                    return true;
                }
            }
            return false;
        }
    }

    Dictionary<string, int> data;

    public Event(EventType eventType, int instigatingNPC, List<int> involvedNPCs, List<int> updatedNPCs, Dictionary<string, int> data)
    {
        this.eventType = eventType;
        this.instigatingNPC = instigatingNPC;
        this.involvedNPCs = involvedNPCs;
        this.updatedNPCs = updatedNPCs;
        this.data = data;
    }

#nullable enable
    public bool TryGetUpdateData(int npc, out EventData? state)
    {
        if (!AllNpcsUpdated)
        {
            if (instigatingNPC == npc || involvedNPCs.Contains(npc))
            {
                updatedNPCs.Add(npc);
                state = new(eventType, instigatingNPC, involvedNPCs, data);
                return true;
            }
        }
        state = null;
        return false;
    }
#nullable disable

}


public class EventData
{
    public readonly EventType eventType;
    public readonly int instigatingNPC;
    public readonly List<int> involvedNPCs;

    public bool IsNPCInstigating(int npc) => instigatingNPC == npc;


    public readonly Dictionary<string, int> data;

    public EventData(EventType eventType, int instigatingNPC, List<int> involvedNPCs, Dictionary<string, int> data)
    {
        this.eventType = eventType;
        this.instigatingNPC = instigatingNPC;
        this.involvedNPCs = involvedNPCs;
        this.data = data;
    }
}

public enum EventType
{
    MeetEvent = 0,
    TradeEvent = 1,
    DonationEvent = 2
}