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
        if (state == State.Spawning)
        {
            DropThenMoveToSpawnPoint();
        }
        else
        {
            base.Update();
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
