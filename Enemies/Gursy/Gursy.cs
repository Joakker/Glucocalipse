using Godot;
using System;

public class Gursy : KinematicBody2D
{
    [Export] public float Speed = 75;
    [Export] public int Health = 2;
    [Export] public bool FaceLeft = false;
	
	private Node target;
	private Vector2 velocity_ = new Vector2();
	
	private Timer AttackTimer;
	private Area2D TriggerZone;

    public override void _Ready()
    {
		// Player detection
		TriggerZone = (Area2D) GetNode("TriggerZone");
		TriggerZone.Connect("body_entered", this, nameof(OnTriggerZoneBodyEntered));
		// Attack animation
		AttackTimer = (Timer) GetNode("AttackTimer");
        AttackTimer.Connect("timeout", this, nameof(OnAttackTimerTimeout));
    }

    public override void _Process(float delta)
    {

    }

	public override void _PhysicsProcess(float delta)
    {
		if (target == null) // If we're not attacking, move
		{
	        int direction = FaceLeft ? -1 : 1;
	        velocity_.x = direction * Speed;
	        MoveAndCollide(velocity_ * delta);
		} else // Else, stop during attack is loading
		{
			MoveAndCollide(velocity_ * 0);
		}
    }

	// Placeholder function. Will be extended.
    public void Attack()
    {
		// Ideally, some kind of animation trigger
		// would be best here, and at the end we should start
		// AttackTimer again if the conditions are still fullfilled.
        GD.Print("Gursy::Attack");
    }
	
	private void OnAttackTimerTimeout()
    {
        Attack();
    }

	private void OnTriggerZoneBodyEntered(Node body)
	{
		AttackTimer.Start(0.5f);
        target = body;
		MoveAndCollide(velocity_ * 0);
	}

}
