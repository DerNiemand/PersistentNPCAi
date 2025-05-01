using Godot;

public partial class QuestGiver : Node
{
	RandomNumberGenerator rng = new();
	[Export]
	QuestManager questManager;
	public Quest GetQuest(Faction faction, Vector2 location)
	{

		if(questManager.LocationNearQuestGiver(faction, location))
		{
			var questLocation = new Vector2(rng.RandfRange(0, 3856),rng.RandfRange(0, 3936));
			return new Quest(questLocation);
		}
		else
		{
			return new Quest(questManager.GetNearestQuestGiverLocation(faction, location));
		}
	}
}
