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
			if (!deletionIndecies.Contains(removeIndex))
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
			var updatedIndex = currentIndex + 1 > offlineNPCs.Count - 1 ? 0 : currentIndex + 1;
			int skippedIndecies = 1;
			while (deletionIndecies.Contains(updatedIndex))
			{
				skippedIndecies++;
				updatedIndex = updatedIndex + 1 > offlineNPCs.Count - 1 ? 0 : updatedIndex + 1;
			}

			frameTimes[updatedIndex] = delta;
			foreach (var time in frameTimes)
			{
				timeSinceLastUpdate += time;
			}

			for(int i = 0; i < skippedIndecies;i++){
				offlineNPCs.RemoveAt(currentIndex + i);
				frameTimes.RemoveAt(currentIndex + i);
				deletionIndecies.Remove(currentIndex + i);
			}

			for (int i = 0; i < deletionIndecies.Count; i++)
			{
				deletionIndecies[i] = deletionIndecies[i] > currentIndex ? deletionIndecies[i] - skippedIndecies : deletionIndecies[i];
			}
			currentIndex = currentIndex >= offlineNPCs.Count - 1 ? 0 : currentIndex;
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
		currentIndex = currentIndex >= offlineNPCs.Count - 1 ? 0 : currentIndex + 1;
	}

	public void OnUpdateTimerTimeout()
	{
		UpdateNPC(updateTime);
	}

}
