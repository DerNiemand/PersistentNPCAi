using Godot;
using System.Collections.Generic;

public partial class QuestManager : Node
{
	[Export]
	TileMapLayer pointsOfInterestLayer;
	[Export]
	QuestGiver questGiver;
	List<Vector2> questGiverLocations = new();
	public override void _Ready()
	{
		var usedCells = pointsOfInterestLayer.GetUsedCells();
		foreach (var cell in usedCells)
		{
			var data = pointsOfInterestLayer.GetCellTileData(cell);
			if (data.HasCustomData("Interactable Identifier") && (string)data.GetCustomData("Interactable Identifier") == "Quest Giver")
			{
				questGiverLocations.Add(pointsOfInterestLayer.ToGlobal(pointsOfInterestLayer.MapToLocal(cell)));
			}
		}
	}

	public Quest GetQuest(Vector2 location)
	{
		return questGiver.GetQuest(location);
	}

	public bool LocationNearQuestGiver(Vector2 location)
	{
		foreach (var giverLocation in questGiverLocations)
		{
			if (giverLocation.DistanceTo(location) <= 10)
			{
				return true;
			}
		}
		return false;
	}

	public Vector2 GetNearestQuestGiverLocation(Vector2 location)
	{
		Vector2 closestLocation = questGiverLocations[0];
		float closestDistance = questGiverLocations[0].DistanceSquaredTo(location);
		for(int i = 1; i < questGiverLocations.Count; i++)
		{
			var thisDistance = questGiverLocations[i].DistanceSquaredTo(location);
			if(thisDistance < closestDistance)
			{
				closestDistance = thisDistance;
				closestLocation = questGiverLocations[i];
			}
		}
		return closestLocation;
	}

}
