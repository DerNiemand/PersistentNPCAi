using Godot;
using System;

public partial class Sword : Weapon
{
	AnimationPlayer AtackAnimPlayer;
	public override void _Ready()
	{
		AtackAnimPlayer = GetNode<AnimationPlayer>("AttackAnimationPlayer");
	}

	public override void Attack(Vector2 direction)
	{
		direction = direction.Normalized();
		if (Mathf.Abs(direction.X) > Mathf.Abs(direction.Y))
		{
			if (direction.X >= 0)
			{
				AtackAnimPlayer.Play("attack_right");
			}
			else
			{
				AtackAnimPlayer.Play("attack_left");
			}
		}
		else
		{
			if (direction.Y >= 0)
			{
				AtackAnimPlayer.Play("attack_down");
			}
			else
			{
				AtackAnimPlayer.Play("attack_up");
			}
		}
		GetNode<Timer>("AttackCooldown").Start(AttackCooldown);
	}

}
