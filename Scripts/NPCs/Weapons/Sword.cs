using Godot;
using System;

public partial class Sword : Weapon
{
	[Signal]
	public delegate void NPCHitEventHandler(NPCBasic hitNPC, int damage);

	[Export]
	int damage = 1;

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

	public void OnBodyEnterDamageArea(Node2D other)
	{
		if (other is NPCBasic)
		{
			EmitSignal(SignalName.NPCHit, other as NPCBasic, damage);
		}
	}

	public override void EnableVisuals()
	{
		GetNode<Sprite2D>("Sprite2D").Visible = true;
	}
    public override void DisableVisuals()
    {
		GetNode<Sprite2D>("Sprite2D").Visible = false;
    }



}
