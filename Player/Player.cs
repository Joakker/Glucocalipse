using Godot;
using System;

public class Player : KinematicBody2D
{
    #region Enums
        private enum State
        {
            MOVE,
            ROLL,
            ATTK,
        };
    #endregion
    #region Exports
        [Export] public int     Acceleration;
        [Export] public int     MaxSpeed;
        [Export] public int     DashSpeed;
        [Export] public float   Friction;
    #endregion

    #region Private Members
        private Vector2 velocity = Vector2.Zero;
        private Vector2 dashDirection = Vector2.Right;

        private State state = State.MOVE;
    #endregion
    public override void _Ready()
    {
        ;
    }

    public override void _PhysicsProcess(float delta)
    {
        switch (state)
        {
            case State.MOVE:
                Move(delta);
                break;
            case State.ROLL:
                Roll(delta);
                break;
        }
    }

    private void Move(float delta)
    {
        var inputVector = Vector2.Zero;
        inputVector.x = Input.GetActionStrength("mv_rt")
                - Input.GetActionStrength("mv_lf");
        inputVector.y = Input.GetActionStrength("mv_dn")
                - Input.GetActionStrength("mv_up");
        inputVector = inputVector.Normalized();
        if (inputVector != Vector2.Zero)
        {
            dashDirection = inputVector;
            velocity = velocity.MoveToward(inputVector * MaxSpeed, Acceleration * delta);
        }
        else
        {
            velocity = velocity.MoveToward(Vector2.Zero, Friction * delta);
        }
        if (Input.IsActionJustPressed("dash"))
            state = State.ROLL;
        velocity = MoveAndSlide(velocity);
    }

    private void Roll(float delta)
    {
        velocity = dashDirection * DashSpeed * 1.5f;
        state = State.MOVE;
    }
}
