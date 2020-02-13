using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Unit
{
    public enum State
    {
        Idle,
        Raking,
        Watering,
        Mining,
        Chopping,
        Building,
        Planting,
        Spawning
    }

    public State state = State.Spawning;

    protected override void Update()
    {
        diff = transform.position - destination;

        if (state == State.Spawning)
        {
            DropThenMoveToSpawnPoint();
        }
        else
        {
            base.Update();
        }
    }

    protected override void Move()
    {
        if (angleToRotate == 0) // facing destination, move forward
        {
            if (Mathf.Abs(diff.x) < 2 && Mathf.Abs(diff.z) < 2)
                StopMoving();
            else
                transform.Translate(velocity * currentSpeed / 10, Space.World);
        }
        else
        {
            base.Move();
        }
    }

    private void DropThenMoveToSpawnPoint()
    {
        if (transform.position.y > 11) // drop
        {
            transform.Translate(0, -.5f, 0);

            if (transform.position.y <= 11) // set on ground
                transform.position = new Vector3(transform.position.x, 11, transform.position.z);
        }
        else if (moving) // move towards destination
        {
            Move();
        }
        else // stop
        {
            state = State.Idle;
        }
    }
}
