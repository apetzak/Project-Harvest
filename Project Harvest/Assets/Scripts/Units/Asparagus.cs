using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asparagus : Veggie
{
    public Burst burst;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void InflictDamage()
    {
        burst.Pop();
        base.InflictDamage();
    }
}