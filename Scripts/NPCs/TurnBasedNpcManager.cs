using System.Collections.Generic;
using Godot;

public partial class TurnBasedNpcManager : Node2D
{
	[Export]
	float updateTime = 1.0f;
	[Export]
	Timer UpdateTimer;

	bool frameBasedUpdating
	{
		get => updateTime <= 0;
	}
	List<TurnBased> offlineNPCs = new();
	List<double> frameTimes = new();
	List<int> deletionIndecies = new();
	int currentIndex = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (!frameBasedUpdating)
		{
			UpdateTimer.Start(updateTime);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (frameBasedUpdating)
		{
			UpdateNPC(delta);
		}
	}

	public void OnNPCOfflineBehaviourEnabled(TurnBased npc)
	{
		if (offlineNPCs.Contains(npc))
		{
			return;
		}
		offlineNPCs.Add(npc);
		frameTimes.Add(0.0);
	}
	public void OnNPCOfflineBehaviourDisabled(TurnBased npc)
	{
		if (offlineNPCs.Contains(npc))
		{
			var removeIndex = offlineNPCs.IndexOf(npc);
			if (deletionIndecies.Contains(removeIndex))
			{
				deletionIndecies.Add(removeIndex);
			}
		}
	}

	public void UpdateNPC(double delta)
	{
		double timeSinceLastUpdate = 0;
		if (deletionIndecies.Contains(currentIndex))
		{
			frameTimes[currentIndex + 1] = delta;
			foreach (var time in frameTimes)
			{
				timeSinceLastUpdate += time;
			}
			offlineNPCs.RemoveAt(currentIndex);
			frameTimes.RemoveAt(currentIndex);
			deletionIndecies.Remove(currentIndex);
			for (int i = 0; i < deletionIndecies.Count; i++)
			{
				deletionIndecies[i] = deletionIndecies[i] > currentIndex ? deletionIndecies[i] - 1 : deletionIndecies[i];
			}
		}
		else
		{
			frameTimes[currentIndex] = delta;
			foreach (var time in frameTimes)
			{
				timeSinceLastUpdate += time;
			}
		}
		offlineNPCs[currentIndex]._Process(timeSinceLastUpdate);
		currentIndex++;
	}

}
