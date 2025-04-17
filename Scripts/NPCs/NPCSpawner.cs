using Godot;
using System;

public partial class NPCSpawner : Node2D
{
	RandomNumberGenerator rng = new();
	[Export]
	PackedScene NPC;
	[Export]
	int amount;
	[Export]
	Rect2 SpawnArea;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for(int i = 0; i < amount; i++)
		{
			Node2D newNPC = NPC.Instantiate() as Node2D;
			var newPos = new Vector2(rng.RandfRange(0,SpawnArea.Size.X), rng.RandfRange(0,SpawnArea.Size.Y));
			newPos += SpawnArea.Position;

			newNPC.Position = newPos;

			AddChild(newNPC,true);
		}
	}

}
