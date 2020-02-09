using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corn : Veggie
{
    public Projectile cannonBall;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (cannonBall.hit)
        {
            cannonBall.hit = false;
            cannonBall.target.health -= attackDamage;
        }
        base.Update();
    }

    public override void InflictDamage()
    {
        cannonBall.Spawn(destination, target);
    }
}
