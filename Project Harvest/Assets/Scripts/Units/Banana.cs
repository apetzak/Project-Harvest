using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : Troop
{
    public Burst burst;

    void Start()
    {
        transform.Rotate(10, 0, 90, Space.World);
        transform.Translate(0, 5f, 0, Space.World);
        base.Start();
    }

    void Update()
    {
        base.Update();
    }

    public override void TriggerAttack()
    {
        burst.Pop();
        base.TriggerAttack();
    }

    public override void RotateTowards(float x, float z)
    {
        float diff = GetAngle(x, z);
        transform.Rotate(diff, 0, 0, Space.Self);
        facingAngle += diff;
    }
}