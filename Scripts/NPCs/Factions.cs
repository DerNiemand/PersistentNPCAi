using System.Collections.Generic;

public enum Faction
{
    Independent = 0,
    Cyan = 1,
    Lime = 2,
    Purple = 3,
    Red = 4

}

public enum Relation
{
    Enemies = 0,
    Neutral = 1,
    Allies = 2
}

public static class FactionRelations
{
    static Dictionary<Faction,Dictionary<Faction,Relation>> factionRelationships = new()
    {
        {
            Faction.Independent, new()
            {
                {Faction.Independent, Relation.Neutral},
                {Faction.Cyan, Relation.Neutral},
                {Faction.Lime, Relation.Neutral},
                {Faction.Purple, Relation.Neutral},
                {Faction.Red,Relation.Neutral}
            }
        },

        {
            Faction.Cyan, new()
            {
                {Faction.Independent, Relation.Neutral},
                {Faction.Cyan, Relation.Allies},
                {Faction.Lime, Relation.Enemies},
                {Faction.Purple, Relation.Allies},
                {Faction.Red,Relation.Neutral}
            }
        },

        {
            Faction.Lime, new()
            {
                {Faction.Independent, Relation.Neutral},
                {Faction.Cyan, Relation.Enemies},
                {Faction.Lime, Relation.Allies},
                {Faction.Purple, Relation.Neutral},
                {Faction.Red,Relation.Allies}
            }
        },

        {
            Faction.Cyan, new()
            {
                {Faction.Independent, Relation.Neutral},
                {Faction.Cyan, Relation.Allies},
                {Faction.Lime, Relation.Neutral},
                {Faction.Purple, Relation.Allies},
                {Faction.Red,Relation.Enemies}
            }
        },

        {
            Faction.Red, new()
            {
                {Faction.Independent, Relation.Neutral},
                {Faction.Cyan, Relation.Neutral},
                {Faction.Lime, Relation.Allies},
                {Faction.Purple, Relation.Enemies},
                {Faction.Red,Relation.Allies}
            }
        }
    };

    public static Relation GetRelation(Faction ownFaction, Faction encounteredFaction)
    {
        return factionRelationships[ownFaction][encounteredFaction];
    }

}