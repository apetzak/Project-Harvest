using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brocolli : Veggie
{
    public Burst burst1;
    public Burst burst2;

    void Start()
    {
        //transform.Rotate(65, 0, 0, Space.World);
        transform.Translate(0, 4, 0, Space.World);
        base.Start();
    }

    void Update()
    {
        base.Update();
    }

    public override void TriggerAttack()
    {
        burst1.Pop();
        burst2.Pop();
        base.TriggerAttack();
    }

    public override void RotateTowards(float x, float z)
    {
        float diff = GetAngle(x, z);
        transform.Rotate(0, diff, 0, Space.World);
        facingAngle += diff;
    }
}
