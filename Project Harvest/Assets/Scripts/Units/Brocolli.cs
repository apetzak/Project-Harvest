using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brocolli : Veggie
{
    public Burst burst1;
    public Burst burst2;

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
        burst1.Pop();
        burst2.Pop();
        base.InflictDamage();
    }
}
