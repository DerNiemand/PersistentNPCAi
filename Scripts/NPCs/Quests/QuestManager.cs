using Godot;
using System.Collections.Generic;

public partial class QuestManager : Node
{
	[Export]
	TileMapLayer pointsOfInterestLayer;
	[Export]
	QuestGiver questGiver;
	Dictionary<Faction, List<Vector2>> questGiverLocations = new();
	public override void _Ready()
	{
		var usedCells = pointsOfInterestLayer.GetUsedCells();
		foreach (var cell in usedCells)
		{
			var data = pointsOfInterestLayer.GetCellTileData(cell);
			if (data.HasCustomData("Interactable Identifier") && (string)data.GetCustomData("Interactable Identifier") == "Quest Giver")
			{
				if (data.HasCustomData("Faction"))
				{
					var faction = (Faction)(int)data.GetCustomData("Faction");
					var location = pointsOfInterestLayer.ToGlobal(pointsOfInterestLayer.MapToLocal(cell));
					if (!questGiverLocations.ContainsKey(faction))
					{
						questGiverLocations.Add(faction, new());
					}

					questGiverLocations[faction].Add(location);

				}

			}
		}
	}

	public Quest GetQuest(Faction faction, Vector2 location)
	{
		return questGiver.GetQuest(faction, location);
	}

	public bool LocationNearQuestGiver(Faction faction, Vector2 location)
	{
		foreach (var giverLocation in questGiverLocations[faction])
		{
			if (giverLocation.DistanceTo(location) <= 10)
			{
				return true;
			}
		}
		return false;
	}

	public Vector2 GetNearestQuestGiverLocation(Faction faction, Vector2 location)
	{
		var factionLocations = questGiverLocations[faction];
		Vector2 closestLocation = factionLocations[0];
		float closestDistance = factionLocations[0].DistanceSquaredTo(location);
		for (int i = 0; i < factionLocations.Count; i++)
		{
			var thisDistance = factionLocations[i].DistanceSquaredTo(location);
			if (thisDistance < closestDistance)
			{
				closestDistance = thisDistance;
				closestLocation = factionLocations[i];
			}
		}
		return closestLocation;
	}

}
