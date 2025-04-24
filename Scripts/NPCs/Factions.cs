using System.Collections.Generic;
using Godot;

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

public static class FactionStats
{
    static SpriteFrames independentFrames = GD.Load<SpriteFrames>("Resources/NPCs/npc_cyan.tres");
    static SpriteFrames cyanFrames = GD.Load<SpriteFrames>("Resources/NPCs/npc_cyan.tres");
    static SpriteFrames limeFrames = GD.Load<SpriteFrames>("Resources/NPCs/npc_lime.tres");
    static SpriteFrames purpleFrames = GD.Load<SpriteFrames>("Resources/NPCs/npc_purple.tres");
    static SpriteFrames redFrames = GD.Load<SpriteFrames>("Resources/NPCs/npc_red.tres");
    static Dictionary<Faction, Dictionary<Faction, Relation>> factionRelationships = new()
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
            Faction.Purple, new()
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

    public static SpriteFrames GetFactionSpriteFrames(Faction faction)
    {
        switch (faction){
            case Faction.Cyan:
                return cyanFrames;
            case Faction.Lime:
                return limeFrames;
            case Faction.Purple:
                return purpleFrames;
            case Faction.Red:
                return redFrames;
            default:
                return independentFrames;
        }
    }

}