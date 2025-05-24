using System;
using System.Collections.Generic;
using Godot;

public partial class EventStore: Node
{
    private Dictionary<int, Event> events = new();

    public void AddEvent(Event eventToAdd)
    {
        events.Add(events.Count, eventToAdd);
    }
#nullable enable
    public List<EventData> GetNewerEventsInvolvingNPC(int startIndex, Guid npc, out int currentIndex)
    {
        List<EventData> retval = new();
        for (int i = startIndex; i < events.Count; i++)
        {
            if (events.TryGetValue(i, out Event? eventToCheck))
            {
                if (eventToCheck.TryGetUpdateData(npc, out EventData? state))
                {
                    retval.Add(state!);
                }
            }
        }
        currentIndex = events.Count;

        return retval;
    }
#nullable disable
}