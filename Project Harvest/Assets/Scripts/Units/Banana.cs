using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : Fruit
{
    public Burst burst;

    protected override void Start()
    {
        transform.Translate(0, 5f, 0, Space.World);
        base.Start();
    }

    public override void InflictDamage()
    {
        burst.Pop();
        base.InflictDamage();
    }
}