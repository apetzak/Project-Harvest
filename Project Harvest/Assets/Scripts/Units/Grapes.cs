using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapes : Fruit
{
    void Start()
    {
        transform.Rotate(0, 0, 180, Space.Self);
        base.Start();
    }

    void Update()
    {
        base.Update();
    }

    public override void RotateTowards(float x, float z)
    {
        float diff = GetAngle(x, z);
        transform.Rotate(0, 0, -diff, Space.Self);
        facingAngle += diff;
    }
}
