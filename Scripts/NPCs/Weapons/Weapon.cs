using System;
using Godot;

public partial class Weapon : Node2D
{
    [Export]
    float range;
    public float Range
    {
        get => range;
    }
    [Export]
    float attackCooldown;
    public float AttackCooldown
    {
        get => attackCooldown;
    }

    public bool AttackCoolingdown
    {
        get => GetNode<Timer>("AttackCooldown").TimeLeft > 0; 
    }

    public virtual void Attack(Vector2 direction) { }

    public virtual void DisableVisuals()
    {
        Visible = false;
    }
    public virtual void EnableVisuals()
    {
        Visible = true;
    }
}
