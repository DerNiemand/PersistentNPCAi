using Godot;
using System;

public partial class Player : Node2D
{
	[Export]
	float speed = 50;
	public override void _Process(double delta)
	{
		var moveY = Input.GetAxis("move_up","move_down");
		var moveX = Input.GetAxis("move_left","move_right");
		Position += (float)delta * speed * new Vector2(moveX,moveY);
	}
}
