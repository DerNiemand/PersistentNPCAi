using Godot;
using System;

public partial class AutoEnd : Node
{
	[Export]
	int runFrameAmount = 300;

	int elapsedFrameCount = 0;

    public override void _Process(double delta)
    {
		elapsedFrameCount++;
		if(elapsedFrameCount > runFrameAmount)
		{
			OnRunOver();
		}
    }


	public void OnRunOver()
	{
		GetTree().Quit();
	}
}
