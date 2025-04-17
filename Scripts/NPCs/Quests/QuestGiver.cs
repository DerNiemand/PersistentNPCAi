using Godot;

public partial class QuestGiver : Node
{
	RandomNumberGenerator rng = new();
	[Export]
	QuestManager questManager;
	public Quest GetQuest(Vector2 location)
	{

		if(questManager.LocationNearQuestGiver(location))
		{
			var questLocation = new Vector2(rng.RandfRange(0, 3856),rng.RandfRange(0, 3936));
			return new Quest(questLocation);
		}
		else
		{
			return new Quest(questManager.GetNearestQuestGiverLocation(location));
		}
	}
}
