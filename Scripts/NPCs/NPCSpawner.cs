using Godot;
using System;

public partial class NPCSpawner : Node2D
{
	RandomNumberGenerator rng = new();
	[Export]
	PackedScene NPC;
	[Export]
	int amount;
	int amountSpawned;
	[Export]
	Rect2 spawnArea;
	[Export]
	bool spawnAllAtStart;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if(spawnAllAtStart)
		{
			for(int i = 0; i < amount; i++)
			{
				NPCBasic newNPC = NPC.Instantiate() as NPCBasic;
				var newPos = new Vector2(rng.RandfRange(0,spawnArea.Size.X), rng.RandfRange(0,spawnArea.Size.Y));
				newPos += spawnArea.Position;

				newNPC.Position = newPos;
				newNPC.Faction = (Faction)rng.RandiRange(1,4);
				AddChild(newNPC,true);
			}
		}
	}

	public override void _Process(double delta)
	{
		if(!spawnAllAtStart && !(amount == amountSpawned))
		{
			NPCBasic newNPC = NPC.Instantiate() as NPCBasic;
			var newPos = new Vector2(rng.RandfRange(0,spawnArea.Size.X), rng.RandfRange(0,spawnArea.Size.Y));
			newPos += spawnArea.Position;
			newNPC.Position = newPos;
			newNPC.Faction = (Faction)rng.RandiRange(1,4);
			AddChild(newNPC,true);
			amountSpawned += 1;
		}
	}

}
