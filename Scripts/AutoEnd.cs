using Godot;
using System;

public partial class AutoEnd : Timer
{
	[Export]
	float runTime = 300;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Start(runTime);
	}

	public void OnTimeOut()
	{
		GetTree().Quit();
	}
}
