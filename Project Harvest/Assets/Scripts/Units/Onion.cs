using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onion : Veggie
{
    public Burst burst;

    void Start()
    {
        transform.Rotate(0, 90, 0, Space.World);
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
}
