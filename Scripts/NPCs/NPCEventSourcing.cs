using Godot;
using System;

public partial class NPCEventSourcing : NPCBasic
{
	private void ChangeState(string state)
	{
		switch (state)
		{
			case "traveling":
				currentState.Exit();
				currentState = new TravelingState();
				currentState.Enter(this);
				break;
			case "combat":
				currentState.Exit();
				currentState = new TradingState();
				currentState.Enter(this);
				break;
			default:
				GD.Print("couldnt find spcified state to switch to");
				break;
		}
	}
}
