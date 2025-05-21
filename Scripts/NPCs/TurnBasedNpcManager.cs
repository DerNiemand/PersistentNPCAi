using System.Collections.Generic;
using Godot;

public partial class TurnBasedNpcManager : Node2D
{
	[Export]
	float updateTime = 1.0f;
	[Export]
	Timer UpdateTimer;

	bool FrameBasedUpdating
	{
		get => updateTime <= 0;
	}
	List<TurnBased> offlineNPCs = new();
	Dictionary<TurnBased, double> frameTimes = new();
	HashSet<TurnBased> deletionNPCs = new();
	int currentIndex = 0;

	public override void _Ready()
	{
		if (!FrameBasedUpdating)
		{
			UpdateTimer.Start(updateTime);
		}
	}

	public override void _Process(double delta)
	{
		if (FrameBasedUpdating)
		{
			UpdateNPC(0.166);
		}
	}

	public void OnNPCOfflineBehaviourEnabled(TurnBased npc)
	{
		if (offlineNPCs.Contains(npc))
		{
			return;
		}
		offlineNPCs.Add(npc);
		frameTimes.Add(npc, 0.0);
	}
	public void OnNPCOfflineBehaviourDisabled(TurnBased npc)
	{
		if (offlineNPCs.Contains(npc))
		{
			if (!deletionNPCs.Contains(npc))
		{
			deletionNPCs.Add(npc);	
			}
		}
	}

	public void UpdateNPC(double delta)
	{
		double timeSinceLastUpdate = 0;
		var updatedNPC = offlineNPCs[currentIndex];
		if (deletionNPCs.Contains(updatedNPC))
		{
			var updatedIndex = currentIndex + 1 > offlineNPCs.Count - 1 ? 0 : currentIndex + 1;
			HashSet<TurnBased> deletedNPCs = [updatedNPC];

			updatedNPC = offlineNPCs[updatedIndex];
			while (deletionNPCs.Contains(updatedNPC))
			{
				deletedNPCs.Add(updatedNPC);
				updatedIndex = updatedIndex + 1 > offlineNPCs.Count - 1 ? 0 : updatedIndex + 1;
				updatedNPC = offlineNPCs[updatedIndex];
			}

			frameTimes[updatedNPC] = delta;
			foreach (var time in frameTimes.Values)
			{
				timeSinceLastUpdate += time;
			}

			foreach (var npc in deletedNPCs)
			{
				offlineNPCs.Remove(npc);
				frameTimes.Remove(npc);
				deletionNPCs.Remove(npc);
			}
		}
		else
		{
			frameTimes[updatedNPC] = delta;
			foreach (var time in frameTimes.Values)
			{
				timeSinceLastUpdate += time;
			}
		}
		updatedNPC._Process(timeSinceLastUpdate);
		currentIndex = currentIndex >= offlineNPCs.Count - 1 ? 0 : currentIndex + 1;
	}

	public void OnUpdateTimerTimeout()
	{
		UpdateNPC(updateTime);
	}

}
