using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corn : Troop
{
    public Projectile cannonBall;

    void Start()
    {
        transform.Rotate(90, 0, 180, Space.Self);
        transform.Translate(0, 5, 0, Space.World);
        base.Start();
    }

    void Update()
    {
        if (cannonBall.hit)
        {
            cannonBall.hit = false;
            cannonBall.target.health -= attackDamage;
        }
        base.Update();
    }

    public override void TriggerAttack()
    {
        cannonBall.Spawn(destination, target);
    }

    public override void RotateTowards(float x, float z)
    {
        float diff = GetAngle(x, z);
        transform.Rotate(0, 0, -diff, Space.Self);
        facingAngle += diff;
    }
}
