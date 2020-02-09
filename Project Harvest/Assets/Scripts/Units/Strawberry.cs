using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strawberry : Fruit
{
    public Projectile arrow;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (arrow.hit)
        {
            arrow.hit = false;
            arrow.target.health -= attackDamage;
        }
        base.Update();
    }

    public override void InflictDamage()
    {
        arrow.Spawn(destination, target);
    }
}
