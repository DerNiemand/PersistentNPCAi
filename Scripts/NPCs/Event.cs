using System;
using System.Collections.Generic;
using System.Linq;

public class Event
{
    private EventType eventType;
    private Guid instigatingNPC;
    private List<Guid> involvedNPCs;
    private List<Guid> updatedNPCs = new();
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

    public Event(EventType eventType, Guid instigatingNPC, List<Guid> involvedNPCs, Dictionary<string, int> data)
    {
        this.eventType = eventType;
        this.instigatingNPC = instigatingNPC;
        this.involvedNPCs = involvedNPCs;
        this.data = data;
    }

#nullable enable
    public bool TryGetUpdateData(Guid npc, out EventData? state)
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
    public readonly Guid instigatingNPC;
    public readonly List<Guid> involvedNPCs;

    public bool IsNPCInstigating(Guid npc) => instigatingNPC == npc;


    public readonly Dictionary<string, int> data;

    public EventData(EventType eventType, Guid instigatingNPC, List<Guid> involvedNPCs, Dictionary<string, int> data)
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